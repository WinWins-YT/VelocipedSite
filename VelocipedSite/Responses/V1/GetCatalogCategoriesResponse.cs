﻿using VelocipedSite.Models;

namespace VelocipedSite.Responses.V1;

public record GetCatalogCategoriesResponse(IEnumerable<CatalogCategory> Categories);