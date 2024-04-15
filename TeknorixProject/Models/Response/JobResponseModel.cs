using System;

public class JobResponseModel
{
    public int Id { get; set; }
    public string Code { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public LocationModel Location { get; set; }
    public DepartmentModel Department { get; set; }
    public DateTime PostedDate { get; set; }
    public DateTime ClosingDate { get; set; }
}

public class LocationModel
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string Country { get; set; }
    public string Zip { get; set; }
}

public class DepartmentModel
{
    public int Id { get; set; }
    public string Title { get; set; }
}
