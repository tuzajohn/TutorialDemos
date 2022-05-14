using PhoneNumbers;

namespace TutorialDemos.PDFConsole.Models;

public class PhoneNumberCheckViewModel
{
    private static PhoneNumberUtil _phoneUtil;
    private string _countryCodeSelected;

    public string CountryCodeSelected
    {
        get => _countryCodeSelected;
        set => _countryCodeSelected = value.ToUpperInvariant();
    }

    public string PhoneNumberRaw { get; }

    // Holds the validation response. Not for data entry.
    public bool Valid { get; }

    // Holds the validation response. Not for data entry.
    public bool HasExtension { get; }


    public PhoneNumberCheckViewModel(string countryCodeSelected, string phoneNumberRaw)
    {
        _phoneUtil = PhoneNumberUtil.GetInstance();

        PhoneNumber phoneNumber = _phoneUtil.Parse(phoneNumberRaw, countryCodeSelected);


        PhoneNumberRaw = phoneNumberRaw;
        CountryCodeSelected = countryCodeSelected;

        Valid = _phoneUtil.IsValidNumberForRegion(phoneNumber, countryCodeSelected);

    }

}
