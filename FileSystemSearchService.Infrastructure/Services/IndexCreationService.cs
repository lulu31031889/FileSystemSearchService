using FileSystemSearchService.Core.Entities;
using FileSystemSearchService.Core.Enums;
using FileSystemSearchService.Core.Interfaces.Services;
using Nest;
using System;

namespace FileSystemSearchService.Infrastructure.Services
{
    public class IndexCreationService : IIndexCreationService
    {
        readonly IElasticClient _elasticClient;

        public IndexCreationService(IElasticClient elasticClient)
        {
            _elasticClient = elasticClient;
        }

        public void CreateAllIndexes()
        {
            foreach (DocumentIndexNames documentIndexName in Enum.GetValues(typeof(DocumentIndexNames)))
            {
                switch (documentIndexName)
                {
                    case DocumentIndexNames.artifacts:
                        CreateArtifactIndex(documentIndexName);
                        break;
                    default:
                        throw new ArgumentException($"No index mapping available for ", documentIndexName.ToString());
                }
            }
        }

        void CreateArtifactIndex(DocumentIndexNames documentIndexName)
        {
            try
            {
                var indexExists = _elasticClient.Indices.Exists(documentIndexName.ToString());

                if (!indexExists.Exists)
                {
                    var createIndexResult = _elasticClient.Indices.Create(DocumentIndexNames.artifacts.ToString(),
                        index => index.Map<Artifact>(map => map
                            .Properties<Artifact>(props => props
                                .Text(p => p
                                    .Name(n => n.FullPath)  //todo: Might make this "search as you type" instead.
                                    )
                                .SearchAsYouType(p => p
                                    .Name(n => n.Name)
                                    .Analyzer("CustomLowerCaseKeywordAnalyzer")
                                    )
                                .Text(p => p
                                    .Name(n => n.Path))
                                .Text(p => p
                                    .Name(n => n.FileType))
                                .Date(p => p
                                    .Name(n => n.Created)
                                    .Format("yyyy-MM-dd'T'HH:mm:ss"))
                                .Date(p => p
                                    .Name(n => n.Modified)
                                    .Format("yyyy-MM-dd'T'HH:mm:ss"))
                                .Date(p => p
                                    .Name(n => n.LastAccessed)
                                    .Format("yyyy-MM-dd'T'HH:mm:ss"))
                                .Number(p => p
                                    .Name(n => n.Bytes)
                                    .Type(NumberType.UnsignedLong)
                                )
                            )
                        )
                        .Settings(s => s
                            .Analysis(a => a
                                .Analyzers(z => z
                                    .Custom("CustomLowerCaseKeywordAnalyzer", c => c
                                        .Tokenizer("keyword")
                                        .Filters("lowercase"))
                                    )
                                )
                            )
                        );
                }
            }
            catch (Exception e)
            {
                //todo: Log this.
                throw;
            }
        }
    }
}
