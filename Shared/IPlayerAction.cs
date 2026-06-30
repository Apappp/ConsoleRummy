using System;
using System.Text.Json.Serialization;

namespace ConsoleRummy
{
    [JsonPolymorphic(TypeDiscriminatorPropertyName = "$type")]
    [JsonDerivedType(typeof(DrawCardAction), "draw")]
    [JsonDerivedType(typeof(DiscardAction), "discard")]

    public interface IPlayerAction
    {
        void ExecuteAction(GameManager table);
    }
}