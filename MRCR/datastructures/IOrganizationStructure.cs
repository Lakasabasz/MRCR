using System;

namespace MRCR.datastructures;

public enum OrganisationObjectType {
    Post,
    Trail,
    Line,
    Control
}

public interface IOrganizationStructure
{
    OrganisationObjectType Type { get; }
    public event EventHandler OnPropertyChanged;
}