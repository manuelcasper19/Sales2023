using Sales.Shared.Entities;
using Sales.API.Services;
using Microsoft.EntityFrameworkCore;
using Sales.Shared.Responses;

namespace Sales.API.Data
{
    public class SeedDb
    {
        private readonly DataContext _context;
        private readonly IApiService _apiService;

        public SeedDb(DataContext context, IApiService apiService)
        {
            _context = context;
            _apiService = apiService;
        }

        public async Task SeedAsync()
        {
            await _context.Database.EnsureCreatedAsync();
            await CheckCountriesAsync();
            await CheckCategoriesAsync();
        }


        private async Task CheckCountriesAsync()
        {
            if (!_context.Countries.Any())
            {
                Response responseCountries = await _apiService.GetListAsync<CountryResponse>("/v1", "/countries");
                if (responseCountries.IsSuccess)
                {
                    List<CountryResponse> countries = (List<CountryResponse>)responseCountries.Result!;
                    foreach (CountryResponse countryResponse in countries)
                    {
                        Country country = await _context.Countries!.FirstOrDefaultAsync(c => c.Name == countryResponse.Name!)!;
                        if (country == null)
                        {
                            country = new() { Name = countryResponse.Name!, States = new List<State>() };
                            Response responseStates = await _apiService.GetListAsync<StateResponse>("/v1", $"/countries/{countryResponse.Iso2}/states");
                            if (responseStates.IsSuccess)
                            {
                                List<StateResponse> states = (List<StateResponse>)responseStates.Result!;
                                foreach (StateResponse stateResponse in states!)
                                {
                                    State state = country.States!.FirstOrDefault(s => s.Name == stateResponse.Name!)!;
                                    if (state == null)
                                    {
                                        state = new() { Name = stateResponse.Name!, Cities = new List<City>() };
                                        Response responseCities = await _apiService.GetListAsync<CityResponse>("/v1", $"/countries/{countryResponse.Iso2}/states/{stateResponse.Iso2}/cities");
                                        if (responseCities.IsSuccess)
                                        {
                                            List<CityResponse> cities = (List<CityResponse>)responseCities.Result!;
                                            foreach (CityResponse cityResponse in cities)
                                            {
                                                if (cityResponse.Name == "Mosfellsbær" || cityResponse.Name == "Șăulița")
                                                {
                                                    continue;
                                                }
                                                City city = state.Cities!.FirstOrDefault(c => c.Name == cityResponse.Name!)!;
                                                if (city == null)
                                                {
                                                    state.Cities.Add(new City() { Name = cityResponse.Name! });
                                                }
                                            }
                                        }
                                        if (state.CitiesNumber > 0)
                                        {
                                            country.States.Add(state);
                                        }
                                    }
                                }
                            }
                            if (country.StatesNumber > 0)
                            {
                                _context.Countries.Add(country);
                                await _context.SaveChangesAsync();
                            }
                        }
                    }
                }
            }

           
        }
        private async Task CheckCategoriesAsync()
        {
            if (!_context.Categories.Any())
            {
                _context.Categories.Add(new Category { Name = "Computadores" });
                _context.Categories.Add(new Category { Name = "Productos de aseo" });
                _context.Categories.Add(new Category { Name = "Ropa" });
                _context.Categories.Add(new Category { Name = "Alimentación" });
                _context.Categories.Add(new Category { Name = "Construcción" });
                _context.Categories.Add(new Category { Name = "Agropecuario" });
                _context.Categories.Add(new Category { Name = "Televisores" });
                _context.Categories.Add(new Category { Name = "Celulares" });
                _context.Categories.Add(new Category { Name = "zapatos" });
                _context.Categories.Add(new Category { Name = "gimnasio" });
                _context.Categories.Add(new Category { Name = "Animales" });
                _context.Categories.Add(new Category { Name = "Gatos" });
                _context.Categories.Add(new Category { Name = "Ropa gato" });
                _context.Categories.Add(new Category { Name = "Alimentación gatos" });
                _context.Categories.Add(new Category { Name = "Alimentación perros" });
                _context.Categories.Add(new Category { Name = "Alimentación Vacas" });
                _context.Categories.Add(new Category { Name = "Alimentación Vacas2" });
                _context.Categories.Add(new Category { Name = "Alimentación colegio" });
                _context.Categories.Add(new Category { Name = "Alimentación equipo futbol" });
                _context.Categories.Add(new Category { Name = "Alimentación equipo Taxistas" });
                _context.Categories.Add(new Category { Name = "Alimentación equipo Uber" });
                _context.Categories.Add(new Category { Name = "Alimentación equipo Indrive" });
                _context.Categories.Add(new Category { Name = "Alimentación equipo Didi" });
                _context.Categories.Add(new Category { Name = "Construcción Edificio" });
                _context.Categories.Add(new Category { Name = "Construcción casa" });
                _context.Categories.Add(new Category { Name = "Construcción piscinas" });
                _context.Categories.Add(new Category { Name = "Construcción canchas" });
                _context.Categories.Add(new Category { Name = "Construcción tenis" });
                _context.Categories.Add(new Category { Name = "Construcción futbol" });
                _context.Categories.Add(new Category { Name = "Construcción beisbol" });
                _context.Categories.Add(new Category { Name = "Construcción softbol" });
                _context.Categories.Add(new Category { Name = "Construcción basket" });
                _context.Categories.Add(new Category { Name = "Construcción obrera" });
                _context.Categories.Add(new Category { Name = "Construcción carreteras" });
                _context.Categories.Add(new Category { Name = "Construcción vías" });
                _context.Categories.Add(new Category { Name = "Agropecuario vacas" });
                _context.Categories.Add(new Category { Name = "Agropecuario perros" });
                _context.Categories.Add(new Category { Name = "Agropecuario marranos" });
                _context.Categories.Add(new Category { Name = "Agropecuario caballos" });
                _context.Categories.Add(new Category { Name = "Agropecuario gallinas" });
                _context.Categories.Add(new Category { Name = "Agropecuario peces" });
                _context.Categories.Add(new Category { Name = "Agropecuario gatos" });
                _context.Categories.Add(new Category { Name = "Agropecuario toros" });
                _context.Categories.Add(new Category { Name = "Agropecuario chorizos" });
                _context.Categories.Add(new Category { Name = "Agropecuario cerdos" });
                _context.Categories.Add(new Category { Name = "Agropecuario moderno" });
                _context.Categories.Add(new Category { Name = "Agropecuario antiguo" });
                _context.Categories.Add(new Category { Name = "Agropecuario bua" });
                _context.Categories.Add(new Category { Name = "Agropecuario religioso" });
                _context.Categories.Add(new Category { Name = "Agropecuario resabiado" });
                _context.Categories.Add(new Category { Name = "Agropecuario rabioso" });
                _context.Categories.Add(new Category { Name = "Agropecuario calmado" });
                _context.Categories.Add(new Category { Name = "Agropecuario iritado" });
                _context.Categories.Add(new Category { Name = "Agropecuario medicado" });
                _context.Categories.Add(new Category { Name = "Agropecuario unido" });
                _context.Categories.Add(new Category { Name = "Agropecuario parto" });
                _context.Categories.Add(new Category { Name = "Televisores nuevo" });
                _context.Categories.Add(new Category { Name = "Televisores viejos" });
                _context.Categories.Add(new Category { Name = "Televisores modernos" });
                _context.Categories.Add(new Category { Name = "Televisores antiguos" });
                _context.Categories.Add(new Category { Name = "Televisores samsung" });
                _context.Categories.Add(new Category { Name = "Televisores lg" });
                _context.Categories.Add(new Category { Name = "Televisores huwei" });
                _context.Categories.Add(new Category { Name = "Televisores kaley" });
                _context.Categories.Add(new Category { Name = "Televisores oi" });
                _context.Categories.Add(new Category { Name = "Televisores barrigones" });
                _context.Categories.Add(new Category { Name = "Televisores LCD" });
                _context.Categories.Add(new Category { Name = "Televisores PLASMA" });
                _context.Categories.Add(new Category { Name = "Televisores SMARTV" });
                _context.Categories.Add(new Category { Name = "Televisores pesados" });
                _context.Categories.Add(new Category { Name = "Televisores livianos" });
                _context.Categories.Add(new Category { Name = "Televisores robados" });
                _context.Categories.Add(new Category { Name = "Celulares nuevo" });
                _context.Categories.Add(new Category { Name = "Celulares viejos" });
                _context.Categories.Add(new Category { Name = "Celulares modernos" });
                _context.Categories.Add(new Category { Name = "Celulares antiguos" });
                _context.Categories.Add(new Category { Name = "Celulares samsung" });
                _context.Categories.Add(new Category { Name = "Celulares lg" });
                _context.Categories.Add(new Category { Name = "Celulares huwei" });
                _context.Categories.Add(new Category { Name = "Celulares kaley" });
                _context.Categories.Add(new Category { Name = "Celulares oi" });
                _context.Categories.Add(new Category { Name = "Celulares nokia" });
                _context.Categories.Add(new Category { Name = "Celulares motorola" });
                _context.Categories.Add(new Category { Name = "Celulares ipro" });
                _context.Categories.Add(new Category { Name = "Celulares iphone" });
                _context.Categories.Add(new Category { Name = "Celulares makui" });
                _context.Categories.Add(new Category { Name = "Celulares 1234" });
                _context.Categories.Add(new Category { Name = "zapatos  nuevos" });
                _context.Categories.Add(new Category { Name = "zapatos  modernos" });
                _context.Categories.Add(new Category { Name = "zapatos  veijos" });
                _context.Categories.Add(new Category { Name = "zapatos  antiguos" });
                _context.Categories.Add(new Category { Name = "zapatos  juveniles" });
                _context.Categories.Add(new Category { Name = "zapatos  clasicos" });
                _context.Categories.Add(new Category { Name = "zapatos  nike" });
                _context.Categories.Add(new Category { Name = "zapatos  addidas" });
                _context.Categories.Add(new Category { Name = "zapatos  lacoste" });
                _context.Categories.Add(new Category { Name = "zapatos  para todo" });
                _context.Categories.Add(new Category { Name = "zapatos  para pelear" });
                _context.Categories.Add(new Category { Name = "zapatos  para chismosear" });
                _context.Categories.Add(new Category { Name = "zapatos  para correr" });
                _context.Categories.Add(new Category { Name = "zapatos  para hablar gatos" });
                _context.Categories.Add(new Category { Name = "zapatos  rotos" });
                _context.Categories.Add(new Category { Name = "gimnasio moderno" });
                _context.Categories.Add(new Category { Name = "gimnasio nuevo" });
                _context.Categories.Add(new Category { Name = "gimnasio viejo" });
                _context.Categories.Add(new Category { Name = "gimnasio antiguo" });
                _context.Categories.Add(new Category { Name = "gimnasio sin pesas" });
                _context.Categories.Add(new Category { Name = "gimnasio sin restriccion" });
                _context.Categories.Add(new Category { Name = "gimnasio con restriccion" });
                _context.Categories.Add(new Category { Name = "gimnasio con volantes" });
                _context.Categories.Add(new Category { Name = "gimnasio sin volantes" });
                _context.Categories.Add(new Category { Name = "gimnasio molestos" });
                _context.Categories.Add(new Category { Name = "gimnasio enojado" });
                _context.Categories.Add(new Category { Name = "gimnasio alegre" });
                _context.Categories.Add(new Category { Name = "gimnasio cansado" });
                _context.Categories.Add(new Category { Name = "gimnasio marca" });
                _context.Categories.Add(new Category { Name = "religion marca" });
                _context.Categories.Add(new Category { Name = "religion turca" });
                _context.Categories.Add(new Category { Name = "religion colombiana" });
                _context.Categories.Add(new Category { Name = "religion venezolana" });
                _context.Categories.Add(new Category { Name = "religion argentina" });
                _context.Categories.Add(new Category { Name = "religion boliviana" });
                _context.Categories.Add(new Category { Name = "religion mexicana" });
                _context.Categories.Add(new Category { Name = "religion panama" });
                _context.Categories.Add(new Category { Name = "religion costarica" });
                _context.Categories.Add(new Category { Name = "religion brasil" });
                _context.Categories.Add(new Category { Name = "religion chile" });
                _context.Categories.Add(new Category { Name = "religion peru" });
                _context.Categories.Add(new Category { Name = "religion hoduras" });
                _context.Categories.Add(new Category { Name = "religion EEUU" });
                _context.Categories.Add(new Category { Name = "religion uruguay" });
                _context.Categories.Add(new Category { Name = "religion paraguay" });
                _context.Categories.Add(new Category { Name = "religion españa" });
                _context.Categories.Add(new Category { Name = "religion francia" });
                _context.Categories.Add(new Category { Name = "religion alemania" });
                _context.Categories.Add(new Category { Name = "religion portugal" });
                _context.Categories.Add(new Category { Name = "religion italia" });
                _context.Categories.Add(new Category { Name = "religion suiza" });
                _context.Categories.Add(new Category { Name = "religion marroqui" });
                _context.Categories.Add(new Category { Name = "religion israel" });
                _context.Categories.Add(new Category { Name = "religion noruega" });
                _context.Categories.Add(new Category { Name = "religion islandia" });
                _context.Categories.Add(new Category { Name = "religion rusia" });
                _context.Categories.Add(new Category { Name = "religion ucrania" });
                _context.Categories.Add(new Category { Name = "religion prosta" });
                _context.Categories.Add(new Category { Name = "religion suecia" });
                _context.Categories.Add(new Category { Name = "religion por no dejar" });
                _context.Categories.Add(new Category { Name = "religion china" });
                _context.Categories.Add(new Category { Name = "religion mongol" });
                _context.Categories.Add(new Category { Name = "religion japon" });
                _context.Categories.Add(new Category { Name = "religion taiwan" });
                _context.Categories.Add(new Category { Name = "religion sparrow" });
                _context.Categories.Add(new Category { Name = "religion marta" });
                _context.Categories.Add(new Category { Name = "religion san andres" });
                _context.Categories.Add(new Category { Name = "neveras antioquia" });
                _context.Categories.Add(new Category { Name = "neveras medellin" });
                _context.Categories.Add(new Category { Name = "neveras bello" });
                _context.Categories.Add(new Category { Name = "neveras sabaneta" });
                _context.Categories.Add(new Category { Name = "neveras envigado" });
                _context.Categories.Add(new Category { Name = "neveras itagui" });
                _context.Categories.Add(new Category { Name = "neveras la estrella" });
                _context.Categories.Add(new Category { Name = "neveras caldas" });
                _context.Categories.Add(new Category { Name = "neveras abejorral" });
                _context.Categories.Add(new Category { Name = "neveras barbosa" });
                _context.Categories.Add(new Category { Name = "neveras cartagena" });
                _context.Categories.Add(new Category { Name = "neveras turbaco" });
                _context.Categories.Add(new Category { Name = "neveras magangue" });
                _context.Categories.Add(new Category { Name = "neveras la isla" });
                _context.Categories.Add(new Category { Name = "neveras bolivar" });
                _context.Categories.Add(new Category { Name = "neveras barranquilla" });
                _context.Categories.Add(new Category { Name = "neveras soldad" });
                _context.Categories.Add(new Category { Name = "neveras puerto colombia" });
                _context.Categories.Add(new Category { Name = "neveras atlantico" });
                _context.Categories.Add(new Category { Name = "neveras cundinamarca" });
                _context.Categories.Add(new Category { Name = "neveras bogota" });
                _context.Categories.Add(new Category { Name = "neveras chia" });
                _context.Categories.Add(new Category { Name = "neveras fusgasuga" });
                _context.Categories.Add(new Category { Name = "neveras el 20" });
                _context.Categories.Add(new Category { Name = "neveras la calera" });
                _context.Categories.Add(new Category { Name = "neveras facatativa" });
                _context.Categories.Add(new Category { Name = "neveras girardot" });
                _context.Categories.Add(new Category { Name = "neveras girardota" });
                _context.Categories.Add(new Category { Name = "neveras usaque" });
                _context.Categories.Add(new Category { Name = "neveras el colegio" });
                _context.Categories.Add(new Category { Name = "neveras el pueblo" });
                _context.Categories.Add(new Category { Name = "neveras la ciudad" });
                _context.Categories.Add(new Category { Name = "neveras la esquina" });
                _context.Categories.Add(new Category { Name = "neveras la extraña" });
                _context.Categories.Add(new Category { Name = "neveras cali" });
                _context.Categories.Add(new Category { Name = "neveras valle" });
                _context.Categories.Add(new Category { Name = "neveras jamundi" });
                _context.Categories.Add(new Category { Name = "neveras palmira" });
                _context.Categories.Add(new Category { Name = "neveras yumbo" });
                _context.Categories.Add(new Category { Name = "neveras candelaria" });
                _context.Categories.Add(new Category { Name = "neveras mercy" });
                _context.Categories.Add(new Category { Name = "neveras albertina" });
                _context.Categories.Add(new Category { Name = "neveras nidia" });
                _context.Categories.Add(new Category { Name = "neveras pasto" });
                _context.Categories.Add(new Category { Name = "neveras nariño" });
                _context.Categories.Add(new Category { Name = "neveras ok" });
                _context.Categories.Add(new Category { Name = "neveras tumaco" });
                _context.Categories.Add(new Category { Name = "neveras tunja" });
                _context.Categories.Add(new Category { Name = "neveras boyaca" });
                _context.Categories.Add(new Category { Name = "neveras segovia" });
                _context.Categories.Add(new Category { Name = "neveras cucuta" });
                _context.Categories.Add(new Category { Name = "neveras norte" });
                _context.Categories.Add(new Category { Name = "neveras bucaramanga " });
                _context.Categories.Add(new Category { Name = "neveras santander " });
                _context.Categories.Add(new Category { Name = "neveras la loma " });
                _context.Categories.Add(new Category { Name = "neveras floridablanca" });
                _context.Categories.Add(new Category { Name = "neveras el banco" });
                _context.Categories.Add(new Category { Name = "neveras santa marta" });
                _context.Categories.Add(new Category { Name = "neveras fundacion" });
                _context.Categories.Add(new Category { Name = "neveras rioacha" });
                _context.Categories.Add(new Category { Name = "neveras valledupar" });
                _context.Categories.Add(new Category { Name = "neveras cesar" });
                _context.Categories.Add(new Category { Name = "neveras codazzi" });
                _context.Categories.Add(new Category { Name = "neveras sicelejo" });
                _context.Categories.Add(new Category { Name = "neveras sucre" });
                _context.Categories.Add(new Category { Name = "neveras el bongo" });



            }
            await _context.SaveChangesAsync();
        }
    }
    }


