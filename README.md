# GIBS.Module.PhotoGallery

An Oqtane module for managing and displaying photo albums with tag filtering, slideshow, and lightbox views.

## Features
- Album and photo management (CRUD)
- Nested albums (sub-albums)
- Tag filtering
- Slideshow view (query-string driven)
- Lightbox view (query-string driven)
- Thumbnail generation and image resizing
- SEO-friendly navigation via query-string links for albums, photos, lightbox, and tag filters

## SEO
- Album, slideshow, lightbox, and tag filtering use hyperlinks with query-string parameters for crawlable URLs.
- Page titles are updated based on the selected album or photo.

## Requirements
- Oqtane Framework (server + client)
- .NET 10

## Installation
1. Build the solution.
2. Package the module using the project under `Package/`.
3. Install the `.nupkg` in Oqtane via the Admin UI.

## Configuration
Module settings are read from Oqtane settings:
- `FileFolderId`
- `ImageThumbWidth`
- `ImageThumbHeight`
- `ImageMaxWidth`
- `ImageMaxHeight`
- `ShowPhotoTitle`

## Usage
- Add the `PhotoGallery` module to a page.
- Use **Manage Gallery** to create albums and upload photos.
- Albums and photos are navigable via query strings for SEO-friendly URLs:
  - Album view: `?album={AlbumId}`
  - Slideshow: `?album={AlbumId}&photo={PhotoId}`
  - Lightbox: `?album={AlbumId}&photo={PhotoId}&lightbox=1`
  - Tag filter: `?album={AlbumId}&tag={TagName}`

## Development
- Client: `Client/`
- Server: `Server/`
- Shared models: `Shared/`
- Packaging: `Package/`

## License
See repository license file.