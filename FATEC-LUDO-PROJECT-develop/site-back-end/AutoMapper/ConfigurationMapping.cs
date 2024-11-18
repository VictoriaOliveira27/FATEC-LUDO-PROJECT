using AutoMapper;

public class ConfigurationMapping : Profile 
{
    public ConfigurationMapping()
    {
        CreateMap<User, ListUsersResponse>().ReverseMap();
        CreateMap<Cosmetic, ListCosmeticsResponse>().ReverseMap();
    }
}