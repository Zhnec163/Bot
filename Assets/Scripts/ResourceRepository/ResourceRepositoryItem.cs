using System;

public class ResourceRepositoryItem
{
    public Base Owner { get; }
    public Resource Resource { get; }
    public bool IsProcessed { get; private set; }

    public ResourceRepositoryItem(Base owner, Resource resource)
    {
        Owner = owner;
        Resource = resource;
    }

    public void Process() => IsProcessed = true;
}
