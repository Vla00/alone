using System;

namespace Одиноко_проживающие
{
    public static class ConfigurationProfram
    {
        //подсключение
        public static string Base { set; get; }
        public static string Source { set; get; }
        public static string Login { set; get; }
        public static string Password { set; get; }

        //конфигурация программы
        public static bool Page { set; get; }
        public static int PageSize { set; get; }
        public static string Theme { set; get; }
    }

    public class StructStartParameters
    {
        public bool Status { set; get; }
        public int KeyAlone { set; get; }
    }

    internal class StructuresAlone : IDisposable
    {
        public string Fio { set; get; }
        public int Pol { set; get; }
        public DateTime DateRo { set; get; }
        public DateTime DateSm { set; get; }
        public string Country { set; get; }
        public string Street { set; get; }
        public string Phone { set; get; }
        public string PlaceWork { set; get; }
        public string SemPol { set; get; }
        public string Dop { set; get; }
        public DateTime DateExit { set; get; }

        public override bool Equals(object obj)
        {
            StructuresAlone alone = obj as StructuresAlone;
            return alone != null && alone.Country == Country && alone.DateRo == DateRo && alone.Fio == Fio &&
                   alone.Pol == Pol && alone.DateSm == DateSm && alone.Street == Street && alone.Phone == Phone &&
                   alone.PlaceWork == PlaceWork && alone.SemPol == SemPol && alone.Dop == Dop && alone.DateExit == DateExit;
        }

        protected bool Equals(StructuresAlone other)
        {
            return string.Equals(Fio, other.Fio) && Pol == other.Pol && DateRo.Equals(other.DateRo) && DateExit.Equals(other.DateExit) && DateSm.Equals(other.DateSm) && string.Equals(Country, other.Country) && string.Equals(Street, other.Street) && string.Equals(Phone, other.Phone) && string.Equals(PlaceWork, other.PlaceWork) && string.Equals(SemPol, other.SemPol) && string.Equals(Dop, other.Dop);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (Fio != null ? Fio.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ Pol;
                hashCode = (hashCode*397) ^ DateRo.GetHashCode();
                hashCode = (hashCode*397) ^ DateSm.GetHashCode();
                hashCode = (hashCode*397) ^ (Country != null ? Country.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (Street != null ? Street.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (Dop != null ? Dop.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (Phone != null ? Phone.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (PlaceWork != null ? PlaceWork.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (SemPol != null ? SemPol.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ DateExit.GetHashCode();
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

    internal class StructuresSojitel : IDisposable
    {
        public string Fio { set; get; }
        public int Pol { set; get; }
        public DateTime DateRo { set; get; }
        public DateTime DateSm { set; get; }
        public string Dop { set; get; }
        public bool _Status { set; get; }

        public override bool Equals(object obj)
        {
            StructuresSojitel sojitel = obj as StructuresSojitel;
            return sojitel != null && sojitel.DateRo == DateRo && sojitel.DateSm == DateSm && sojitel.Dop == Dop &&
                   sojitel.Fio == Fio && sojitel.Pol == Pol;
        }

        protected bool Equals(StructuresSojitel other)
        {
            return string.Equals(Fio, other.Fio) && Pol == other.Pol && DateRo.Equals(other.DateRo) && DateSm.Equals(other.DateSm) && string.Equals(Dop, other.Dop);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (Fio != null ? Fio.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ Pol;
                hashCode = (hashCode*397) ^ DateRo.GetHashCode();
                hashCode = (hashCode*397) ^ DateSm.GetHashCode();
                hashCode = (hashCode*397) ^ (Dop != null ? Dop.GetHashCode() : 0);
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

    internal class StructuresJilUsl : IDisposable
    {
        public int CountRoom { set; get; }
        public string Place { set; get; }
        public string Woter { set; get; }
        public string Plita { set; get; }
        public string Kanal { set; get; }
        public string Otopl { set; get; }
        public bool _Status { set; get; }


        public override bool Equals(object obj)
        {
            StructuresJilUsl jilUsl = obj as StructuresJilUsl;
            return jilUsl != null && jilUsl.CountRoom == CountRoom && jilUsl.Place == Place &&
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
        public bool Podsobn { set; get; }
        public bool Zemeln { set; get; }
        public string Place { set; get; }
        public string Status { set; get; }
        public int Api { set; get; }
        public int Szu { set; get; }
        public bool _Status { set; get; }

        public override bool Equals(object obj)
        {
            StructuresZemeln zemeln = obj as StructuresZemeln;
            return zemeln != null && zemeln.Podsobn == Podsobn && zemeln.Zemeln == Zemeln &&
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
}
