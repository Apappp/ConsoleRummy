using System;
using System.Text.Json.Serialization;

namespace ConsoleRummy
{
    [JsonPolymorphic(TypeDiscriminatorPropertyName = "$type")]
    [JsonDerivedType(typeof(DrawCardAction), "draw")]
    [JsonDerivedType(typeof(DiscardAction), "discard")]
    [JsonDerivedType(typeof(SwapCardsAction), "swap")]

    public interface IPlayerAction
    {
        void ExecuteAction(GameManager table);
    }
}