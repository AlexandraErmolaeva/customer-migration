using System.Xml.Serialization;

namespace Infrastructure.HostedServices.Dtos;

[XmlRoot(ElementName = "Customers")]
public class CustomerRawDtoList
{
    [XmlElement(ElementName = "Customer")]
    public List<CustomerRawDto> Customers { get; set; }
}

public class CustomerRawDto
{
    [XmlElement(ElementName = "CardCode")]
    public string CardCode { get; set; }

    [XmlElement(ElementName = "LastName")]
    public string LastName { get; set; }

    [XmlElement(ElementName = "FirstName")]
    public string FirstName { get; set; }

    [XmlElement(ElementName = "SurName")]
    public string SurName { get; set; }

    [XmlElement(ElementName = "Gender")]
    public string Gender { get; set; }

    [XmlElement(ElementName = "Birthday")]
    public string Birthday { get; set; }

    [XmlElement(ElementName = "City")]
    public string City { get; set; }

    [XmlElement(ElementName = "PhoneMobile")]
    public string PhoneMobile { get; set; }

    [XmlElement(ElementName = "Email")]
    public string Email { get; set; }

    [XmlElement(ElementName = "Pincode")]
    public string Pincode { get; set; }

    [XmlElement(ElementName = "Bonus")]
    public string Bonus { get; set; }

    [XmlElement(ElementName = "Turnover")]
    public string Turnover { get; set; }
}
