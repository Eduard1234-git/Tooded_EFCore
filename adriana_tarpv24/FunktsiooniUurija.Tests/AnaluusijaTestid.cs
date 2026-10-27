using FunktsiooniUurija.Models;
using FunktsiooniUurija.Services;
using Xunit;

namespace FunktsiooniUurija.Tests;

public class AnaluusijaTestid
{
    // Abimeetod: täidab uurimise valitud arvutustega
    private static FunktsiooniUurimine Uuri(string valem, params string[] valik)
    {
        var o = new FunktsiooniUurimine { Valem = valem };
        Analuusija.Taida(o, Valikud.Sulund(valik));
        return o;
    }

    [Fact]
    public void Tuletis_KuupFunktsioon_TagastabOigeVaartuse()
    {
        // f(x) = x^3 - 3x, seega f'(2) = 3*2^2 - 3 = 9
        var o = Uuri("x^3 - 3*x", "tuletis");
        double tulemus = Analuusija.Kompileeri(o.Tuletis)(2);
        Assert.Equal(9, tulemus, 3);
    }

    [Fact]
    public void Nullkohad_KuupFunktsioon_LeiabKolmJuurt()
    {
        // x^3 - 3x = 0  =>  x = -√3; 0; √3
        var o = Uuri("x^3 - 3*x", "null");
        Assert.Equal("x = -1.7321; x = 0; x = 1.7321", o.Nullkohad);
    }

    [Fact]
    public void Ekstreemumid_RuutFunktsioon_LeiabMiinimumi()
    {
        // x^2 - 4x + 3: kriitiline punkt x = 2, min (2; -1)
        var o = Uuri("x^2 - 4*x + 3", "krit", "ekst");
        Assert.Equal("x = 2", o.KriitilisedPunktid);
        Assert.Equal("min: (2; -1)", o.Ekstreemumid);
    }

    [Fact]
    public void Ekstreemumid_x3Miinus12x_LeiabMaksimumiJaMiinimumi()
    {
        // x^3 - 12x: kriitilised x = ±2, max (-2; 16), min (2; -16)
        var o = Uuri("x^3 - 12*x", "krit", "ekst");
        Assert.Equal("x = -2; x = 2", o.KriitilisedPunktid);
        Assert.Equal("max: (-2; 16); min: (2; -16)", o.Ekstreemumid);
    }

    [Fact]
    public void Normaliseeri_LisabKorrutusmargid()
    {
        Assert.Equal("2*x^2+3*x", Analuusija.Normaliseeri("2x^2 + 3x"));
    }

    // ---- Hüpoteesi kontrolltestid: koodi lugemise põhjal oodatakse, et need kukuvad läbi ----

    [Fact]
    public void Normaliseeri_Log10_EiLisaKorrutusmarki()
    {
        // Regex (\d)(x|\() asendab "0(" → "0*(" ja rikub funktsiooni nime log10
        Assert.Equal("log10(x)", Analuusija.Normaliseeri("log10(x)"));
    }

    [Fact]
    public void KriitilisedPunktid_KolmekordneJuur_LeiabPunktiXOnKolmandik()
    {
        // (3x-1)^3 = 27x^3 - 27x^2 + 9x - 1; f'(x) = 9(3x-1)^2 = 0 kui x = 1/3 (topeltjuur, märk ei muutu)
        var o = Uuri("27*x^3 - 27*x^2 + 9*x - 1", "krit");
        Assert.Equal("x = 0.3333", o.KriitilisedPunktid);
    }
}
