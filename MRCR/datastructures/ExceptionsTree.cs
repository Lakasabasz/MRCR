using System;

namespace MRCR.datastructures;

public class DataStructureException : Exception { }
public class WorldOrganisationException : DataStructureException { }
public class PostsCollisionException : WorldOrganisationException { }