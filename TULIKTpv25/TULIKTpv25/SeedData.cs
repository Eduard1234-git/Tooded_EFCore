namespace Tooded_EFCore;

// NB! Hinnad on ORIENTEERUVAD (Rimi/Coop tasemel, km-ga). Enne esitamist kontrolli rimi.ee / coop.ee.
public static class SeedData
{
    public static readonly string[] Kategooriad =
    {
        "Joogid", "Puu- ja köögiviljad", "Maiustused", "Piimatooted ja munad",
        "Leib ja saiakesed", "Liha ja kala", "Kuivained ja kohv"
    };

    // kategooria, nimi, kogus laos, hind, vana hind (soodus) või null
    private static readonly (int Kat, string Nimi, int Kogus, decimal Hind, decimal? Vana)[] Andmed =
    {
        (4,"Piim Alma 2,5% 1,5 l",48,1.39m,null), (4,"Piim Tere 2,5% 1 l",60,1.19m,null),
        (4,"Kohupiim Alma 9% 350 g",25,1.99m,null), (4,"Hapukoor Alma 20% 400 g",30,1.79m,2.09m),
        (4,"Munad M/L Põltsamaa 10 tk",40,3.29m,null), (4,"Või Tere 82% 200 g",22,3.19m,null),
        (4,"Viilujuust Hollandi E-Piim 150 g",35,2.19m,null), (4,"Jogurt Alma maasika 150 g",80,0.69m,null),
        (5,"Rukkileib Leibur 330 g",30,1.39m,null), (5,"Sepik Eesti Pagar 400 g",28,1.49m,null),
        (5,"Tostisai Eesti Pagar 500 g",20,1.69m,1.99m), (5,"Kaneelisaiake",15,0.99m,null),
        (5,"Croissant",18,0.89m,null),
        (2,"Banaan 1 kg",70,1.79m,null), (2,"Õun Poola 1 kg",55,1.59m,null),
        (2,"Kurk kiles 1 tk",40,1.49m,null), (2,"Tomat kobar 500 g",26,2.49m,2.99m),
        (2,"Kartul 2 kg",90,2.29m,null), (2,"Sidrun 1 tk",60,0.49m,null),
        (2,"Porgand 1 kg",75,1.09m,null), (2,"Avokaado 1 tk",0,1.49m,null),
        (6,"Broileri rinnafilee Rakvere 600 g",20,5.99m,null), (6,"Hakkliha Rakvere 500 g",18,4.69m,null),
        (6,"Viiner Rakvere 300 g",32,2.49m,null), (6,"Lõhefilee Norra 300 g",12,6.99m,8.49m),
        (6,"Sink Rakvere viilutatud 150 g",24,2.29m,null),
        (1,"Coca-Cola 1,5 l",50,2.19m,null), (1,"Fanta Orange 1,5 l",4,2.19m,null),
        (1,"Värska Originaal vesi 1,5 l",100,0.89m,null), (1,"Tropicana apelsinimahl 1 l",28,2.39m,null),
        (1,"Monster Energy 0,5 l",36,1.79m,2.09m),
        (3,"Milka piimašokolaad 100 g",45,1.99m,null), (3,"Kalev piimašokolaad 100 g",40,1.89m,null),
        (3,"Kinder Bueno 43 g",60,1.19m,null), (3,"Mars 51 g",70,0.99m,null),
        (3,"Haribo Goldbears 175 g",33,1.79m,null), (3,"Pringles Original 165 g",24,3.09m,3.69m),
        (3,"Lay's soolane 130 g",35,2.19m,null),
        (7,"Barilla Spaghetti 500 g",50,1.99m,null), (7,"Basmati riis 1 kg",30,3.19m,null),
        (7,"Kaerahelbed 500 g",45,0.99m,null), (7,"Dansukker suhkur 1 kg",60,1.59m,null),
        (7,"Päevalilleõli 1 l",34,2.29m,null), (7,"Tatar 800 g",29,2.49m,null),
        (7,"Felix ketšup 485 g",22,2.79m,null), (7,"Paulig Presidentti kohv 450 g",16,6.99m,null),
    };

    public static IEnumerable<Kategooria> KategooriadList() =>
        Kategooriad.Select((n, i) => new Kategooria { Id = i + 1, Kategooria_nimetus = n });

    public static IEnumerable<Toode> Tooted() =>
        Andmed.Select((x, i) => new Toode
        {
            Id = i + 1, Toodenimetus = x.Nimi, Kogus = x.Kogus, Hind = x.Hind, VanaHind = x.Vana,
            KategooriaId = x.Kat, Pilt = $"toode_{i + 1}.png"
        });
}
