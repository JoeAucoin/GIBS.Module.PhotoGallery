using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Oqtane.Enums;
using Oqtane.Infrastructure;
using Oqtane.Models;
using Oqtane.Security;
using Oqtane.Shared;
using GIBS.Module.PhotoGallery.Models;
using GIBS.Module.PhotoGallery.Repository;
using Oqtane.Repository;
using Oqtane.Services;
using SixLabors.ImageSharp;

namespace GIBS.Module.PhotoGallery.Services
{
    public class ServerPhotoService : IPhotoService
    {
        private readonly IPhotoRepository _photoRepository;
        private readonly IAlbumRepository _albumRepository;
        private readonly IFileRepository _files;
        private readonly IImageService _imageService;
        private readonly IUserPermissions _userPermissions;
        private readonly ILogManager _logger;
        private readonly IHttpContextAccessor _accessor;
        private readonly Alias _alias;

        public ServerPhotoService(IPhotoRepository photoRepository, IAlbumRepository albumRepository, IFileRepository files, IImageService imageService, IUserPermissions userPermissions, ITenantManager tenantManager, ILogManager logger, IHttpContextAccessor accessor)
        {
            _photoRepository = photoRepository;
            _albumRepository = albumRepository;
            _files = files;
            _imageService = imageService;
            _userPermissions = userPermissions;
            _logger = logger;
            _accessor = accessor;
            _alias = tenantManager.GetAlias();
        }

        public Task<List<PhotoAlbum>> GetPhotoAlbumsAsync(int ModuleId)
        {
            if (_userPermissions.IsAuthorized(_accessor.HttpContext.User, _alias.SiteId, EntityNames.Module, ModuleId, PermissionNames.View))
            {
                var photos = _photoRepository.GetPhotos(ModuleId).ToList();
                var albums = _albumRepository.GetAlbums(ModuleId)
                    .Select(album =>
                    {
                        var albumPhotos = photos.Where(photo => photo.AlbumId == album.AlbumId)
                            .OrderBy(photo => photo.SortOrder)
                            .ToList();

                        var thumbnailPath = albumPhotos.FirstOrDefault()?.ThumbnailPath ?? albumPhotos.FirstOrDefault()?.FilePath;
                        var filePath = albumPhotos.FirstOrDefault()?.FilePath ?? albumPhotos.FirstOrDefault()?.ThumbnailPath;

                        return new PhotoAlbum
                        {
                            AlbumId = album.AlbumId,
                            AlbumName = album.AlbumName,
                            Description = album.Description,
                            FilePath = filePath,
                            ThumbnailPath = thumbnailPath,
                            ItemCount = albumPhotos.Count
                        };
                    })
                    .ToList();

                return Task.FromResult(albums);
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized PhotoAlbum Get Attempt {ModuleId}", ModuleId);
                return null;
            }
        }

        public Task<List<Photo>> GetPhotosAsync(int ModuleId)
        {
            if (_userPermissions.IsAuthorized(_accessor.HttpContext.User, _alias.SiteId, EntityNames.Module, ModuleId, PermissionNames.View))
            {
                return Task.FromResult(_photoRepository.GetPhotos(ModuleId).ToList());
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized Photo Get Attempt {ModuleId}", ModuleId);
                return null;
            }
        }

        public Task<Photo> GetPhotoAsync(int PhotoId, int ModuleId)
        {
            if (_userPermissions.IsAuthorized(_accessor.HttpContext.User, _alias.SiteId, EntityNames.Module, ModuleId, PermissionNames.View))
            {
                return Task.FromResult(_photoRepository.GetPhoto(PhotoId));
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized Photo Get Attempt {PhotoId} {ModuleId}", PhotoId, ModuleId);
                return null;
            }
        }

        public Task<Photo> AddPhotoAsync(Photo Photo)
        {
            if (_userPermissions.IsAuthorized(_accessor.HttpContext.User, _alias.SiteId, EntityNames.Module, Photo.ModuleId, PermissionNames.Edit))
            {
                Photo = _photoRepository.AddPhoto(Photo);
                _logger.Log(LogLevel.Information, this, LogFunction.Create, "Photo Added {Photo}", Photo);
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized Photo Add Attempt {Photo}", Photo);
                Photo = null;
            }
            return Task.FromResult(Photo);
        }

        public Task<Photo> UpdatePhotoAsync(Photo Photo)
        {
            if (_userPermissions.IsAuthorized(_accessor.HttpContext.User, _alias.SiteId, EntityNames.Module, Photo.ModuleId, PermissionNames.Edit))
            {
                Photo = _photoRepository.UpdatePhoto(Photo);
                _logger.Log(LogLevel.Information, this, LogFunction.Update, "Photo Updated {Photo}", Photo);
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized Photo Update Attempt {Photo}", Photo);
                Photo = null;
            }
            return Task.FromResult(Photo);
        }

        public Task DeletePhotoAsync(int PhotoId, int ModuleId)
        {
            if (_userPermissions.IsAuthorized(_accessor.HttpContext.User, _alias.SiteId, EntityNames.Module, ModuleId, PermissionNames.Edit))
            {
                _photoRepository.DeletePhoto(PhotoId);
                _logger.Log(LogLevel.Information, this, LogFunction.Delete, "Photo Deleted {PhotoId}", PhotoId);
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized Photo Delete Attempt {PhotoId} {ModuleId}", PhotoId, ModuleId);
            }
            return Task.CompletedTask;
        }

        public async Task<ThumbnailResponse> CreateImageThumbnailAsync(int fileId, int width, int height, int moduleId)
        {
            if (!_userPermissions.IsAuthorized(_accessor.HttpContext.User, _alias.SiteId, EntityNames.Module, moduleId, PermissionNames.Edit))
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized Thumbnail Creation Attempt for FileId {FileId}", fileId);
                return null;
            }

            var originalFile = _files.GetFile(fileId);
            if (originalFile == null)
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Read, "File Not Found For Thumbnail Creation {FileId}", fileId);
                return null;
            }

            string thumbFilePath = null;
            try
            {
                string originalFilePath = _files.GetFilePath(originalFile);
                if (System.IO.File.Exists(originalFilePath))
                {
                    string thumbFileName = $"{System.IO.Path.GetFileNameWithoutExtension(originalFile.Name)}_thumb.{originalFile.Extension}";
                    string folderPath = System.IO.Path.GetDirectoryName(originalFilePath);
                    thumbFilePath = System.IO.Path.Combine(folderPath, thumbFileName);

                    var resizedImagePath = _imageService.CreateImage(originalFilePath, width, height, "medium", "center", "white", "", originalFile.Extension, thumbFilePath);

                    if (string.IsNullOrEmpty(resizedImagePath) || !System.IO.File.Exists(resizedImagePath))
                    {
                        throw new System.Exception("Image resizing failed. Resized file was not created.");
                    }

                    var fileInfo = new System.IO.FileInfo(resizedImagePath);
                    int imageWidth;
                    int imageHeight;
                    using (var image = await Image.LoadAsync(resizedImagePath))
                    {
                        imageWidth = image.Width;
                        imageHeight = image.Height;
                    }

                    var thumbFile = new Oqtane.Models.File
                    {
                        FolderId = originalFile.FolderId,
                        Name = thumbFileName,
                        Extension = originalFile.Extension,
                        Size = (int)fileInfo.Length,
                        ImageHeight = imageHeight,
                        ImageWidth = imageWidth
                    };

                    var newFile = _files.AddFile(thumbFile);
                    _logger.Log(LogLevel.Information, this, LogFunction.Create, "Thumbnail Created Successfully for FileId {FileId}. New FileId is {NewFileId}", fileId, newFile.FileId);

                    return new ThumbnailResponse { FileId = newFile.FileId, Url = newFile.Url };
                }
                else
                {
                    _logger.Log(LogLevel.Error, this, LogFunction.Read, "File Not Found For Resizing {FileId}", fileId);
                    return null;
                }
            }
            catch (System.Exception ex)
            {
                if (!string.IsNullOrEmpty(thumbFilePath) && System.IO.File.Exists(thumbFilePath))
                {
                    System.IO.File.Delete(thumbFilePath);
                }
                _logger.Log(LogLevel.Error, this, LogFunction.Create, ex, "Error Creating Thumbnail for FileId {FileId}", fileId);
                return null;
            }
        }

        public Task<ThumbnailResponse> ResizeImageAsync(int fileId, int width, int height, int moduleId)
        {
            if (!_userPermissions.IsAuthorized(_accessor.HttpContext.User, _alias.SiteId, EntityNames.Module, moduleId, PermissionNames.Edit))
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized Image Resize Attempt for FileId {FileId}", fileId);
                return Task.FromResult<ThumbnailResponse>(null);
            }

            var originalFile = _files.GetFile(fileId);
            if (originalFile == null)
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Read, "File Not Found For Image Resize {FileId}", fileId);
                return Task.FromResult<ThumbnailResponse>(null);
            }

            string resizedFilePath = null;
            try
            {
                var originalFilePath = _files.GetFilePath(originalFile);
                if (System.IO.File.Exists(originalFilePath))
                {
                    var folderPath = System.IO.Path.GetDirectoryName(originalFilePath);
                    var resizedFileName = $"{System.IO.Path.GetFileNameWithoutExtension(originalFile.Name)}_resized.{originalFile.Extension}";
                    resizedFilePath = System.IO.Path.Combine(folderPath, resizedFileName);

                    int targetWidth = width;
                    int targetHeight = height;
                    using (var image = Image.Load(originalFilePath))
                    {
                        var widthRatio = (double)width / image.Width;
                        var heightRatio = (double)height / image.Height;
                        var scale = System.Math.Min(widthRatio, heightRatio);

                        targetWidth = (int)System.Math.Max(1, System.Math.Round(image.Width * scale));
                        targetHeight = (int)System.Math.Max(1, System.Math.Round(image.Height * scale));
                    }

                    var resizedImagePath = _imageService.CreateImage(originalFilePath, targetWidth, targetHeight, "medium", "center", "white", "", originalFile.Extension, resizedFilePath);
                    if (string.IsNullOrEmpty(resizedImagePath) || !System.IO.File.Exists(resizedImagePath))
                    {
                        throw new System.Exception("Image resizing failed. Resized file was not created.");
                    }

                    System.IO.File.Copy(resizedImagePath, originalFilePath, true);
                    System.IO.File.Delete(resizedImagePath);

                    var fileInfo = new System.IO.FileInfo(originalFilePath);
                    using (var image = Image.Load(originalFilePath))
                    {
                        originalFile.ImageWidth = image.Width;
                        originalFile.ImageHeight = image.Height;
                    }

                    originalFile.Size = (int)fileInfo.Length;
                    _files.UpdateFile(originalFile);

                    _logger.Log(LogLevel.Information, this, LogFunction.Update, "Image Resized Successfully for FileId {FileId}", fileId);
                    return Task.FromResult(new ThumbnailResponse { FileId = originalFile.FileId, Url = originalFile.Url });
                }

                _logger.Log(LogLevel.Error, this, LogFunction.Read, "File Not Found For Image Resize {FileId}", fileId);
                return Task.FromResult<ThumbnailResponse>(null);
            }
            catch (System.Exception ex)
            {
                if (!string.IsNullOrEmpty(resizedFilePath) && System.IO.File.Exists(resizedFilePath))
                {
                    System.IO.File.Delete(resizedFilePath);
                }
                _logger.Log(LogLevel.Error, this, LogFunction.Update, ex, "Error Resizing Image for FileId {FileId}", fileId);
                return Task.FromResult<ThumbnailResponse>(null);
            }
        }
    }
}
