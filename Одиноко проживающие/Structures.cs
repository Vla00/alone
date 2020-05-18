using System;
using System.Data.SqlClient;

namespace Одиноко_проживающие
{
    public class ConfigurationProgram
    {
        //подключение
        public string Base { set; get; }
        public string Source { set; get; }
        public string Port { set; get; }
        public string Login { set; get; }
        public string Password { set; get; }

        //конфигурация программы
        public string User { set; get; }
        public string AutoSearch { set; get; }
        public bool Page { set; get; }
        public int PageSize { set; get; }
        public string Theme { set; get; }

        public string NameComp { set; get; }
    }

    public class StructStartParameters
    {
        public bool Status { set; get; }
        public int KeyAlone { set; get; }
        public string Item { set; get; }
        public bool? Blocked { set; get; }
    }

    public class ConfigurationProgramConn
    {
        public ConfigurationProgram configurationProgram;
        public SqlConnection sqlConnection;
        public SqlConnectionStringBuilder connectionStringBuilder;
        public string message;
        public bool connect;
    }

    internal class StructuresAlone : IDisposable
    {
        protected bool Equals(StructuresAlone other)
        {
            return string.Equals(Family, other.Family) && string.Equals(Name, other.Name) && string.Equals(Surname, other.Surname) && Pol == other.Pol && DateRo.Equals(other.DateRo) && DateSm.Equals(other.DateSm) && string.Equals(Country, other.Country) && string.Equals(TypeUl, other.TypeUl) && string.Equals(Street, other.Street) && string.Equals(House, other.House) && string.Equals(Apartament, other.Apartament) && string.Equals(Housing, other.Housing) && string.Equals(Phone, other.Phone) && string.Equals(PlaceWork, other.PlaceWork) && string.Equals(Dop, other.Dop) && DateExit.Equals(other.DateExit);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == this.GetType() && Equals((StructuresAlone) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (Family != null ? Family.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Name != null ? Name.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Surname != null ? Surname.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ Pol;
                hashCode = (hashCode * 397) ^ DateRo.GetHashCode();
                hashCode = (hashCode * 397) ^ DateSm.GetHashCode();
                hashCode = (hashCode * 397) ^ (Country != null ? Country.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (TypeUl != null ? TypeUl.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Street != null ? Street.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (House != null ? House.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Apartament != null ? Apartament.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Housing != null ? Housing.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Phone != null ? Phone.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (PlaceWork != null ? PlaceWork.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Dop != null ? Dop.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ DateExit.GetHashCode();
                return hashCode;
            }
        }

        public string Family { set; get; }
        public string Name { set; get; }
        public string Surname { set; get; }
        //public string Fio { set; get; }
        public int Pol { set; get; }
        public DateTime DateRo { set; get; }
        public DateTime DateSm { set; get; }
        public string Country { set; get; }

        public string TypeUl { set; get; }
        public string Street { set; get; }
        public string House { set; get; }
        public string Apartament { set; get; }
        public string Housing { set; get; }


        public string Phone { set; get; }
        public string PlaceWork { set; get; }
        public string Dop { set; get; }
        public DateTime DateExit { set; get; }

        public string Users { set; get; }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
    }

    internal class StructuresSojitel : IDisposable
    {
        protected bool Equals(StructuresSojitel other)
        {
            return string.Equals(Family, other.Family) && string.Equals(Name, other.Name) && string.Equals(Surname, other.Surname) && Pol == other.Pol && DateRo.Equals(other.DateRo) && DateSm.Equals(other.DateSm) && string.Equals(Dop, other.Dop);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == this.GetType() && Equals((StructuresSojitel) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (Family != null ? Family.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Name != null ? Name.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Surname != null ? Surname.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ Pol;
                hashCode = (hashCode * 397) ^ DateRo.GetHashCode();
                hashCode = (hashCode * 397) ^ DateSm.GetHashCode();
                hashCode = (hashCode * 397) ^ (Dop != null ? Dop.GetHashCode() : 0);
                return hashCode;
            }
        }

        public string Family { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }

        public int Pol { get; set; }
        public DateTime DateRo { get; set; }
        public DateTime DateSm { get; set; }
        public string Dop { get; set; }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
    }

    internal class StructuresJilUsl : IDisposable
    {
        public int CountRoom { get; set; }
        public string Place { get; set; }
        public string Woter { set; get; }
        public string Plita { set; get; }
        public string Kanal { set; get; }
        public string Otopl { set; get; }
        public bool _Status { set; get; }


        public override bool Equals(object obj)
        {
            return obj is StructuresJilUsl jilUsl && jilUsl.CountRoom == CountRoom && jilUsl.Place == Place &&
                   jilUsl.Woter == Woter && jilUsl.Plita == Plita && jilUsl.Kanal == Kanal && jilUsl.Otopl == Otopl;
        }

        protected bool Equals(StructuresJilUsl other)
        {
            return CountRoom == other.CountRoom && string.Equals(Place, other.Place) && string.Equals(Woter, other.Woter) && string.Equals(Plita, other.Plita) && string.Equals(Kanal, other.Kanal) && string.Equals(Otopl, other.Otopl);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = CountRoom;
                hashCode = (hashCode*397) ^ (Place != null ? Place.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (Woter != null ? Woter.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (Plita != null ? Plita.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (Kanal != null ? Kanal.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (Otopl != null ? Otopl.GetHashCode() : 0);
                return hashCode;
            }
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
    }

    internal class StructuresZemeln : IDisposable
    {
        public bool Podsobn { get; set; }
        public bool Zemeln { get; set; }
        public string Place { get; set; }
        public string Status { get; set; }
        public int Api { get; set; }
        public int Szu { get; set; }
        public bool _Status { set; get; }

        public override bool Equals(object obj)
        {
            return obj is StructuresZemeln zemeln && zemeln.Podsobn == Podsobn && zemeln.Zemeln == Zemeln &&
                   zemeln.Place == Place && zemeln.Status == Status && zemeln.Api == Api && zemeln.Szu == Szu;
        }

        protected bool Equals(StructuresZemeln other)
        {
            return Podsobn == other.Podsobn && Zemeln == other.Zemeln && string.Equals(Place, other.Place) && string.Equals(Status, other.Status) && Api == other.Api && Szu == other.Szu;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Podsobn.GetHashCode();
                hashCode = (hashCode*397) ^ Zemeln.GetHashCode();
                hashCode = (hashCode*397) ^ (Place != null ? Place.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (Status != null ? Status.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ Api;
                hashCode = (hashCode*397) ^ Szu;
                return hashCode;
            }
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
    }

    internal class StructuresAlones : IDisposable
    {
        public StructuresAlone Alone { set; get; }
        public StructuresSojitel Sojitel { set; get; }
        public StructuresInvalidnost Invalidnost { set; get; }
        public StructuresFamily Family { set; get; }
        public void Dispose()
        {
            GC.SuppressFinalize(this);
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
    }

    internal class StructuresInvalidnost : IDisposable
    {
        public string Stepen { get; set; }
        public string Date_start { set; get; }
        public string Date_pere { set; get; }
        public string Diagnoz { set; get; }
        public void Dispose()
        {
            GC.SuppressFinalize(this);
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
    }

    internal class StructuresFamily : IDisposable
    {
        public string FioMather { set; get; }
        public string FioFather { set; get; }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
    }
}
