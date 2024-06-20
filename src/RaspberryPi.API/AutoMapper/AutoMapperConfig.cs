namespace RaspberryPi.API.AutoMapper
{
    public static class AutoMapperConfig
    {
        public static Type[] RegisterMappings()
        {
            return new Type[]
            {
                typeof(ViewModelToDomainMappingProfile),
                typeof(DomainToViewModelMappingProfile),
            };
        }
    }
}
