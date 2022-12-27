using System;

public interface IShuffle
{
    void Shuffle();
}

[Serializable] public class IShuffleContainer : IUnifiedContainer<IShuffle> { }