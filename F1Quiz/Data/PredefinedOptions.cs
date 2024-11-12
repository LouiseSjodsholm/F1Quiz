using F1Quiz.Models;

namespace F1Quiz.Data
{
    public static class PredefinedOptions
    {
        public static List<DriverOption> AllDrivers = new List<DriverOption>
        {
            //Red Bull
            new DriverOption{Name="Max Verstappen", ImagePath="/images/drivers/maxVerstappen.png"},
            new DriverOption{Name="Sergio Perez", ImagePath="/images/drivers/sergioPerez.png"},
            //McLaren
            new DriverOption{Name="Lando Norris", ImagePath="/images/drivers/landoNorris.png"},
            new DriverOption{Name="Oscar Piastri", ImagePath="/images/drivers/oscarPiastri.png"},
            //Ferrari
            new DriverOption{Name="Charles Leclerc", ImagePath="/images/drivers/charlesLeclerc.png"},
            new DriverOption{Name="Carlos Sainz", ImagePath="/images/drivers/carlosSainz.png"},
            //Mercedes
            new DriverOption{Name="George Russell", ImagePath="/images/drivers/georgeRussell.png"},
            new DriverOption{Name="Lewis Hamilton", ImagePath="/images/drivers/lewisHamilton.png"},
            //Aston Martin
            new DriverOption{Name="Fernando Alonso", ImagePath="/images/drivers/fernandoAlonso.png"},
            new DriverOption{Name="Lance Stroll", ImagePath="/images/drivers/lanceStroll.png"},
            //Alpine
            new DriverOption{Name="Pierre Gasly", ImagePath="/images/drivers/pierreGasly.png"},
            new DriverOption{Name="Esteban Ocon", ImagePath="/images/drivers/estebanOcon.png"},
            //Haas
            new DriverOption{Name="Nico Hulkenberg", ImagePath="/images/drivers/nicoHulkenberg.png"},
            new DriverOption{Name="Kevin Magnussen", ImagePath="/images/drivers/kevinMagnussen.png"},
            //RB
            new DriverOption{Name="Yuki Tsunoda", ImagePath="/images/drivers/yukiTsunoda.png"},
            new DriverOption{Name="Liam Lawson", ImagePath="/images/drivers/liamLawson.png"},
            //Williams
            new DriverOption{Name="Alexander Albon", ImagePath="/images/drivers/alexAlbon.png"},
            new DriverOption{Name="Franco Colapinto", ImagePath="/images/drivers/francoColapinto.png"},
            //Kick Sauber
            new DriverOption{Name="Valtteri Bottas", ImagePath="/images/drivers/valtteriBottas.png"},
            new DriverOption{Name="Zhou Guanyu", ImagePath="/images/drivers/zhouGuanyu.png"}
        };
    }
}
