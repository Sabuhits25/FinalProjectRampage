﻿using AutoMapper;
using Rampage.BLL.DTO_s.Commons;
using Rampage.BLL.Services.Interfaces;

namespace Rampage.BLL.MappingProfiles.Commons
{
    public class CustomMappingAction<TSource, TDestination> : IMappingAction<TSource, TDestination> where TSource : IAuditedEntityDTO
    {
        private readonly IFileManagerService _fileManagerService;

        public CustomMappingAction(IFileManagerService fileManagerService)
        {
            _fileManagerService = fileManagerService;
        }

        public void Process(TSource source, TDestination destination, ResolutionContext context)
        {
            if (source.Image != null)
            {
                var uploadedUrl = _fileManagerService.UploadFileAsync(source.Image).Result;
                var property = destination.GetType().GetProperty("ImageUrl");

                if (property != null && property.PropertyType == typeof(string))
                {
                    property.SetValue(destination, uploadedUrl);
                }
            }
        }
    }
}
