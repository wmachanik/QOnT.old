
namespace QOnT.App_Code
{
  public class ContactDetails
  {
    public class ContactEmailDetails
    {
      string _FirstName, _LastName, _EmailAddress;
      string _altFirstName, _altLastName, _altEmailAddress;

      public ContactEmailDetails()
      {
        _FirstName = _LastName = _EmailAddress = "";
        _altFirstName = _altLastName = _altEmailAddress = "";
      }
      public string FirstName { get { return _FirstName; } set { _FirstName = value; } }
      public string LastName { get { return _LastName; } set { _LastName = value; } }
      public string EmailAddress { get { return _EmailAddress; } set { _EmailAddress = value; } }
      public string altFirstName { get { return _altFirstName; } set { _altFirstName = value; } }
      public string altLastName { get { return _altLastName; } set { _altLastName = value; } }
      public string altEmailAddress { get { return _altEmailAddress; } set { _altEmailAddress = value; } }
    }

    public ContactDetails()
    {

    }
  }
}