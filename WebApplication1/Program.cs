using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

var mailRepository = new MailRepository();

mailRepository.Add(new Mail(14213, "Таганрог", "Гагаринская", 3, 5, 52, ""));


app.MapGet("/", () =>
{

    return mailRepository.GetAll()
        .Select(mail => new
        {
            mail.Index,
            mail.City,
            mail.Street,
            mail.House,
            mail.Korpus,
            mail.Appartment
        });
});
app.MapPost("/", (Mail newMail) =>
{
    mailRepository.Add(newMail);
    return Results.Created("/", newMail);
});

app.MapPut("/{index}", (int index, Mail updatedMail) =>
{
    var existingMail = mailRepository.GetByIndex(index);

    if (existingMail == null)
    {
        return Results.NotFound("Письмо не найдено");
    }

    existingMail.City = updatedMail.City;
    existingMail.Street = updatedMail.Street;
    existingMail.House = updatedMail.House;
    existingMail.Korpus = updatedMail.Korpus;
    existingMail.Appartment = updatedMail.Appartment;
    existingMail.Letter = updatedMail.Letter;

    return Results.Ok(existingMail);
});

app.MapDelete("/{index}", (int index) =>
{
    var mailToRemove = mailRepository.GetByIndex(index);

    if (mailToRemove == null)
    {
        return Results.NotFound("Письмо не найдено");
    }

    mailRepository.Remove(mailToRemove);
    return Results.Ok("Письмо успешно удалено");
});

app.Run();


class Mail
{
    int index;
    string city;
    string street;
    int house;
    int korpus;
    int appartment;
    string letter;



    public Mail(int index, string city, string street, int house, int korpus, int appartment, string letter)
    {
        Index = index;
        City = city;
        Street = street;
        House = house;
        Korpus = korpus;
        Appartment = appartment;
        Letter = letter;
    }

    public int Index { get => index; set => index = value; }
    public string City { get => city; set => city = value; }
    public string Street { get => street; set => street = value; }
    public int House { get => house; set => house = value; }
    public int Korpus { get => korpus; set => korpus = value; }
    public int Appartment { get => appartment; set => appartment = value; }
    public string Letter { get => letter; set => letter = value; }


    public string GetAddress()
    {
        return $"{City}, {Street}, {House}, {Korpus}, {Appartment}";
    }
}


class MailRepository
{
    public readonly List<Mail> _mails;

    public MailRepository()
    {
        _mails = new List<Mail>();
    }


    public List<Mail> GetAll()
    {
        return _mails;
    }


    public void Add(Mail mail)
    {
        _mails.Add(mail);
    }


    public Mail GetByIndex(int index)
    {
        return _mails.FirstOrDefault(mail => mail.Index == index);
    }


    public bool Remove(Mail mail)
    {
        return _mails.Remove(mail);
    }
}
