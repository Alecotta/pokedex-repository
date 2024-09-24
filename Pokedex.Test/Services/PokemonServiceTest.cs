using AutoMapper;
using Moq;
using Pokedex.WebApi.DTOs.Response;
using Pokedex.WebApi.Infrastructure.ExternalServices;
using Pokedex.WebApi.Models;
using Pokedex.WebApi.Models.PokeApi;
using Pokedex.WebApi.Models.Translation;
using Pokedex.WebApi.Services;
using System.Net;

namespace Pokedex.Test.Services
{
    public class PokemonServiceTest
    {
        [Fact]
        public async Task GetPokemonByNameAsync_ThrowArgumentNullExceptionWhenPokemonNameIsNull()
        {
            //Arrange
            string? pokemonName = null;
            var mockPokeApiService = new Mock<IPokeApiService>();
            var mockTranslatorService = new Mock<ITranslatorService>();
            var mockMapper = new Mock<IMapper>();
            var pokemonService = new PokemonService(mockPokeApiService.Object, mockTranslatorService.Object, mockMapper.Object);

            //Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await pokemonService.GetPokemonByNameAsync(pokemonName));
        }

        [Fact]
        public async Task GetPokemonByNameAsync_ThrowArgumentNullExceptionWhenPokemonNameIsEmpty()
        {
            //Arrange
            string pokemonName = string.Empty;
            var mockPokeApiService = new Mock<IPokeApiService>();
            var mockTranslatorService = new Mock<ITranslatorService>();
            var mockMapper = new Mock<IMapper>();
            var pokemonService = new PokemonService(mockPokeApiService.Object, mockTranslatorService.Object, mockMapper.Object);

            //Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await pokemonService.GetPokemonByNameAsync(pokemonName));
        }

        [Fact]
        public async Task GetPokemonByNameAsync_ReturnSuccessTrueAndResultNotNullWhenPokemonFoundAndStatusCode200()
        {
            //Arrange
            string pokemonName = "pokemon_test";
            var pokemonSpecieModelMock = new PokemonSpecieModel
            {
                Name = pokemonName,
                IsLegendary = true,
                FlavorTextEntries = new List<FlavorTextEntry>
                    {
                        new FlavorTextEntry
                        {
                            FlavorText = "flavor_text_test"
                        }
                    },
                Habitat = new Habitat
                {
                    Name = "habitat_test"
                }
            };
            var pokemonResponseDTOMock = new PokemonResponseDTO
            {
                Description = "flavor_text_test",
                Habitat = "habitat_test",
                IsLegendary = true,
                Name = pokemonName
            };

            var mockPokeApiService = new Mock<IPokeApiService>();

            mockPokeApiService.Setup(m => m.GetPokemonSpecieModelByNameAsync(pokemonName))
                .ReturnsAsync(ResultModel<PokemonSpecieModel?>.Success(pokemonSpecieModelMock, HttpStatusCode.OK));

            var mockMapper = new Mock<IMapper>();
            var mockTranslatorService = new Mock<ITranslatorService>();
            mockMapper.Setup(m => m.Map<PokemonResponseDTO>(It.IsAny<PokemonSpecieModel>())).Returns(pokemonResponseDTOMock);
            var pokemonService = new PokemonService(mockPokeApiService.Object, mockTranslatorService.Object, mockMapper.Object);

            //Act
            var getPokemonByNameAsyncResult = await pokemonService.GetPokemonByNameAsync(pokemonName);

            //Act & Assert
            Assert.True(getPokemonByNameAsyncResult.IsSuccess);
            Assert.NotNull(getPokemonByNameAsyncResult.Data);
            Assert.Equal(getPokemonByNameAsyncResult.StatusCode, HttpStatusCode.OK);
            Assert.Equal(getPokemonByNameAsyncResult.Data.Name, pokemonName);
            Assert.Equal(getPokemonByNameAsyncResult.Data.IsLegendary, pokemonResponseDTOMock.IsLegendary);
            Assert.Equal(getPokemonByNameAsyncResult.Data.Habitat, pokemonResponseDTOMock.Habitat);
            Assert.Equal(getPokemonByNameAsyncResult.Data.Description, pokemonResponseDTOMock.Description);
        }

        [Fact]
        public async Task GetPokemonByNameAsync_ReturnSuccessFalseAndResultNullWhenPokemonNotFound()
        {
            //Arrange
            string pokemonName = "pokemon_test";

            var mockPokeApiService = new Mock<IPokeApiService>();
            var mockTranslatorService = new Mock<ITranslatorService>();
            mockPokeApiService.Setup(m => m.GetPokemonSpecieModelByNameAsync(pokemonName))
                .ReturnsAsync(ResultModel<PokemonSpecieModel?>.Failure("Pokemon Not Found.", HttpStatusCode.NotFound));

            var mockMapper = new Mock<IMapper>();
            var pokemonService = new PokemonService(mockPokeApiService.Object, mockTranslatorService.Object, mockMapper.Object);

            //Act
            var getPokemonByNameAsyncResult = await pokemonService.GetPokemonByNameAsync(pokemonName);

            //Act & Assert
            Assert.False(getPokemonByNameAsyncResult.IsSuccess);
            Assert.Null(getPokemonByNameAsyncResult.Data);
            Assert.Equal(getPokemonByNameAsyncResult.StatusCode, HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task GetPokemonByNameAsync_ReturnSuccessFalseAndResultNullWhenExternalServiceNotWorking()
        {
            //Arrange
            string pokemonName = "pokemon_test";

            var mockPokeApiService = new Mock<IPokeApiService>();
            var mockTranslatorService = new Mock<ITranslatorService>();
            mockPokeApiService.Setup(m => m.GetPokemonSpecieModelByNameAsync(pokemonName))
                .ReturnsAsync(ResultModel<PokemonSpecieModel?>.Failure("Bad Gateway.", HttpStatusCode.BadGateway));

            var mockMapper = new Mock<IMapper>();
            var pokemonService = new PokemonService(mockPokeApiService.Object, mockTranslatorService.Object, mockMapper.Object);

            //Act
            var getPokemonByNameAsyncResult = await pokemonService.GetPokemonByNameAsync(pokemonName);

            //Act & Assert
            Assert.False(getPokemonByNameAsyncResult.IsSuccess);
            Assert.Null(getPokemonByNameAsyncResult.Data);
            Assert.Equal(getPokemonByNameAsyncResult.StatusCode, HttpStatusCode.BadGateway);
        }

        [Fact]
        public async Task GetPokemonByNameAsync_WhenRequiredTranslationInvokeExternalService()
        {
            //Arrange
            string pokemonName = "pokemon_test";

            var mockPokeApiService = new Mock<IPokeApiService>();
            var mockPokeApiServiceResponse = new PokemonSpecieModel
            {
                Name = pokemonName,
                IsLegendary = false,
                Habitat = new Habitat
                {
                    Name = "habitat_test"
                },
                FlavorTextEntries = new List<FlavorTextEntry>
                    {
                        new FlavorTextEntry
                        {
                            FlavorText = "test_flavor_text"
                        }
                    }
            };
            var mockTranslatorService = new Mock<ITranslatorService>();
            mockTranslatorService.Setup(m => m.GetTraslation(mockPokeApiServiceResponse.FlavorTextEntries.First().FlavorText, It.IsAny<TranslationType>()))
                .ReturnsAsync(ResultModel<TranslationModel?>.Success(new TranslationModel
                {
                    Success = new Success
                    {
                        Total = 1
                    },
                    Contents = new Contents
                    {
                        Text = "text_test",
                        Translated = "translated_test",
                        Translation = "translation_test"
                    }
                }));
            mockPokeApiService.Setup(m => m.GetPokemonSpecieModelByNameAsync(pokemonName))
                .ReturnsAsync(ResultModel<PokemonSpecieModel?>.Success(mockPokeApiServiceResponse));

            var mockMapper = new Mock<IMapper>();
            mockMapper.Setup(m => m.Map<PokemonResponseDTO>(It.IsAny<PokemonSpecieModel>())).Returns(new PokemonResponseDTO
            {
                Description = mockPokeApiServiceResponse.FlavorTextEntries.First().FlavorText,
                Habitat = mockPokeApiServiceResponse.Habitat.Name,
                IsLegendary = mockPokeApiServiceResponse.IsLegendary,
                Name = mockPokeApiServiceResponse.Name
            });
            var pokemonService = new PokemonService(mockPokeApiService.Object, mockTranslatorService.Object, mockMapper.Object);

            //Act
            var getPokemonByNameAsyncResult = await pokemonService.GetPokemonByNameAsync(pokemonName, true);

            //Assert
            mockTranslatorService.Verify(s => s.GetTraslation(It.IsAny<string>(), It.IsAny<TranslationType>()), Times.Once);
        }

        [Fact]
        public async Task GetPokemonByNameAsync_WhenRequiredTranslationAndPokemonLegendaryInvokeExternalServiceWithShakespearTranslation()
        {
            //Arrange
            string pokemonName = "pokemon_test";

            var mockPokeApiService = new Mock<IPokeApiService>();
            var mockPokeApiServiceResponse = new PokemonSpecieModel
            {
                Name = pokemonName,
                IsLegendary = true,
                Habitat = new Habitat
                {
                    Name = "habitat_test"
                },
                FlavorTextEntries = new List<FlavorTextEntry>
                    {
                        new FlavorTextEntry
                        {
                            FlavorText = "test_flavor_text"
                        }
                    }
            };
            var mockTranslatorService = new Mock<ITranslatorService>();
            mockTranslatorService.Setup(m => m.GetTraslation(mockPokeApiServiceResponse.FlavorTextEntries.First().FlavorText, TranslationType.SHAKESPEARE))
                .ReturnsAsync(ResultModel<TranslationModel?>.Success(new TranslationModel
                {
                    Success = new Success
                    {
                        Total = 1
                    },
                    Contents = new Contents
                    {
                        Text = "text_test",
                        Translated = "translated_test",
                        Translation = "translation_test"
                    }
                }));
            mockPokeApiService.Setup(m => m.GetPokemonSpecieModelByNameAsync(pokemonName))
                .ReturnsAsync(ResultModel<PokemonSpecieModel?>.Success(mockPokeApiServiceResponse));

            var mockMapper = new Mock<IMapper>();
            mockMapper.Setup(m => m.Map<PokemonResponseDTO>(It.IsAny<PokemonSpecieModel>())).Returns(new PokemonResponseDTO
            {
                Description = mockPokeApiServiceResponse.FlavorTextEntries.First().FlavorText,
                Habitat = mockPokeApiServiceResponse.Habitat.Name,
                IsLegendary = mockPokeApiServiceResponse.IsLegendary,
                Name = mockPokeApiServiceResponse.Name
            });
            var pokemonService = new PokemonService(mockPokeApiService.Object, mockTranslatorService.Object, mockMapper.Object);

            //Act
            var getPokemonByNameAsyncResult = await pokemonService.GetPokemonByNameAsync(pokemonName, true);

            //Act & Assert
            mockTranslatorService.Verify(s => s.GetTraslation(mockPokeApiServiceResponse.FlavorTextEntries.First().FlavorText, TranslationType.SHAKESPEARE), Times.Once);
        }

        [Fact]
        public async Task GetPokemonByNameAsync_WhenRequiredTranslationAndPokemonHabitatIsNotCaveAndIsNotLegendaryInvokeExternalServiceWithYodaTranslation()
        {
            //Arrange
            string pokemonName = "pokemon_test";

            var mockPokeApiService = new Mock<IPokeApiService>();
            var mockPokeApiServiceResponse = new PokemonSpecieModel
            {
                Name = pokemonName,
                IsLegendary = false,
                Habitat = new Habitat
                {
                    Name = "not_cave"
                },
                FlavorTextEntries = new List<FlavorTextEntry>
                    {
                        new FlavorTextEntry
                        {
                            FlavorText = "test_flavor_text"
                        }
                    }
            };
            var mockTranslatorService = new Mock<ITranslatorService>();
            mockTranslatorService.Setup(m => m.GetTraslation(mockPokeApiServiceResponse.FlavorTextEntries.First().FlavorText, It.IsAny<TranslationType>()))
                .ReturnsAsync(ResultModel<TranslationModel?>.Success(new TranslationModel
                {
                    Success = new Success
                    {
                        Total = 1
                    },
                    Contents = new Contents
                    {
                        Text = "text_test",
                        Translated = "translated_test",
                        Translation = "translation_test"
                    }
                }));
            mockPokeApiService.Setup(m => m.GetPokemonSpecieModelByNameAsync(pokemonName))
                .ReturnsAsync(ResultModel<PokemonSpecieModel?>.Success(mockPokeApiServiceResponse));

            var mockMapper = new Mock<IMapper>();
            mockMapper.Setup(m => m.Map<PokemonResponseDTO>(It.IsAny<PokemonSpecieModel>())).Returns(new PokemonResponseDTO
            {
                Description = mockPokeApiServiceResponse.FlavorTextEntries.First().FlavorText,
                Habitat = mockPokeApiServiceResponse.Habitat.Name,
                IsLegendary = mockPokeApiServiceResponse.IsLegendary,
                Name = mockPokeApiServiceResponse.Name
            });
            var pokemonService = new PokemonService(mockPokeApiService.Object, mockTranslatorService.Object, mockMapper.Object);

            //Act
            var getPokemonByNameAsyncResult = await pokemonService.GetPokemonByNameAsync(pokemonName, true);

            //Act & Assert
            mockTranslatorService.Verify(s => s.GetTraslation(mockPokeApiServiceResponse.FlavorTextEntries.First().FlavorText, TranslationType.YODA), Times.Once);
        }

        [Fact]
        public async Task GetPokemonByNameAsync_WhenRequiredTranslationAndTraslationServiceNotErrorThenChangeDescriptionWithYoda()
        {
            //Arrange
            string pokemonName = "pokemon_test";

            var mockPokeApiService = new Mock<IPokeApiService>();
            var mockPokeApiServiceResponse = new PokemonSpecieModel
            {
                Name = pokemonName,
                IsLegendary = false,
                Habitat = new Habitat
                {
                    Name = "not_cave"
                },
                FlavorTextEntries = new List<FlavorTextEntry>
                    {
                        new FlavorTextEntry
                        {
                            FlavorText = "test_flavor_text"
                        }
                    }
            };
            var mockTranslatorService = new Mock<ITranslatorService>();
            var mockTranslatorServiceResponseShakespear = new TranslationModel
            {
                Success = new Success
                {
                    Total = 1
                },
                Contents = new Contents
                {
                    Text = "text_test_shakespear",
                    Translated = "translated_test_shakespear",
                    Translation = "translation_test_shakespear"
                }
            };
            var mockTranslatorServiceResponseYoda = new TranslationModel
            {
                Success = new Success
                {
                    Total = 1
                },
                Contents = new Contents
                {
                    Text = "text_test_yoda",
                    Translated = "translated_test_yoda",
                    Translation = "translation_test_yoda"
                }
            };
            mockTranslatorService.Setup(m => m.GetTraslation(mockPokeApiServiceResponse.FlavorTextEntries.First().FlavorText, TranslationType.SHAKESPEARE))
                .ReturnsAsync(ResultModel<TranslationModel?>.Success(mockTranslatorServiceResponseShakespear));
            mockTranslatorService.Setup(m => m.GetTraslation(mockPokeApiServiceResponse.FlavorTextEntries.First().FlavorText, TranslationType.YODA))
               .ReturnsAsync(ResultModel<TranslationModel?>.Success(mockTranslatorServiceResponseYoda));
            mockPokeApiService.Setup(m => m.GetPokemonSpecieModelByNameAsync(pokemonName))
                .ReturnsAsync(ResultModel<PokemonSpecieModel?>.Success(mockPokeApiServiceResponse));

            var mockMapper = new Mock<IMapper>();
            mockMapper.Setup(m => m.Map<PokemonResponseDTO>(It.IsAny<PokemonSpecieModel>())).Returns(new PokemonResponseDTO
            {
                Description = mockPokeApiServiceResponse.FlavorTextEntries.First().FlavorText,
                Habitat = mockPokeApiServiceResponse.Habitat.Name,
                IsLegendary = mockPokeApiServiceResponse.IsLegendary,
                Name = mockPokeApiServiceResponse.Name
            });
            var pokemonService = new PokemonService(mockPokeApiService.Object, mockTranslatorService.Object, mockMapper.Object);

            //Act
            var getPokemonByNameAsyncResult = await pokemonService.GetPokemonByNameAsync(pokemonName, true);

            //Act & Assert
            Assert.NotNull(getPokemonByNameAsyncResult.Data);
            Assert.Equal(getPokemonByNameAsyncResult.Data.Description, mockTranslatorServiceResponseYoda.Contents.Translated);
        }

        [Fact]
        public async Task GetPokemonByNameAsync_WhenRequiredTranslationAndTraslationServiceNotErrorThenChangeDescriptionWithShakespear()
        {
            //Arrange
            string pokemonName = "pokemon_test";

            var mockPokeApiService = new Mock<IPokeApiService>();
            var mockPokeApiServiceResponse = new PokemonSpecieModel
            {
                Name = pokemonName,
                IsLegendary = true,
                Habitat = new Habitat
                {
                    Name = "not_cave"
                },
                FlavorTextEntries = new List<FlavorTextEntry>
                    {
                        new FlavorTextEntry
                        {
                            FlavorText = "test_flavor_text"
                        }
                    }
            };
            var mockTranslatorService = new Mock<ITranslatorService>();
            var mockTranslatorServiceResponseShakespear = new TranslationModel
            {
                Success = new Success
                {
                    Total = 1
                },
                Contents = new Contents
                {
                    Text = "text_test_shakespear",
                    Translated = "translated_test_shakespear",
                    Translation = "translation_test_shakespear"
                }
            };
            var mockTranslatorServiceResponseYoda = new TranslationModel
            {
                Success = new Success
                {
                    Total = 1
                },
                Contents = new Contents
                {
                    Text = "text_test_yoda",
                    Translated = "translated_test_yoda",
                    Translation = "translation_test_yoda"
                }
            };
            mockTranslatorService.Setup(m => m.GetTraslation(mockPokeApiServiceResponse.FlavorTextEntries.First().FlavorText, TranslationType.SHAKESPEARE))
                .ReturnsAsync(ResultModel<TranslationModel?>.Success(mockTranslatorServiceResponseShakespear));
            mockTranslatorService.Setup(m => m.GetTraslation(mockPokeApiServiceResponse.FlavorTextEntries.First().FlavorText, TranslationType.YODA))
               .ReturnsAsync(ResultModel<TranslationModel?>.Success(mockTranslatorServiceResponseYoda));
            mockPokeApiService.Setup(m => m.GetPokemonSpecieModelByNameAsync(pokemonName))
                .ReturnsAsync(ResultModel<PokemonSpecieModel?>.Success(mockPokeApiServiceResponse));

            var mockMapper = new Mock<IMapper>();
            mockMapper.Setup(m => m.Map<PokemonResponseDTO>(It.IsAny<PokemonSpecieModel>())).Returns(new PokemonResponseDTO
            {
                Description = mockPokeApiServiceResponse.FlavorTextEntries.First().FlavorText,
                Habitat = mockPokeApiServiceResponse.Habitat.Name,
                IsLegendary = mockPokeApiServiceResponse.IsLegendary,
                Name = mockPokeApiServiceResponse.Name
            });
            var pokemonService = new PokemonService(mockPokeApiService.Object, mockTranslatorService.Object, mockMapper.Object);

            //Act
            var getPokemonByNameAsyncResult = await pokemonService.GetPokemonByNameAsync(pokemonName, true);

            //Act & Assert
            Assert.NotNull(getPokemonByNameAsyncResult.Data);
            Assert.Equal(getPokemonByNameAsyncResult.Data.Description, mockTranslatorServiceResponseShakespear.Contents.Translated);
        }
    }
}
