from random import * #randit
from math import *
#Ülesanne 1
print ("Tere, maailm")
nimi = input("Siseta ona nimi: ")
print(f"Tere, maailm! Tervitan sind {nimi}!")
try:
    vanus=int(input("Siseta oma vanus: "))
    print(f"Tere, maailm! Tervitan sind {nimi}! Sa oled {vanus} aastat vana.")
except ValueError:
    print("Palun siseta kehtiv number vanuse jaoks. ")


#Ülesanne 2
vanus=18
eesnimi="Katya"
pikkus=16.5
kas_käib_koolis=True
print(f"Vanus:{vanus}, tüüp: {type(vanus)}")
print(f"Eesnimi:{eesnimi}, tüüp: {type(eesnimi)}")
print(f"Pikkus:{pikkus}, tüüp: {type(pikkus)}")
print(f"Käib koolis:{kas_käib_koolis}, tüüp: {type(kas_käib_koolis)}")


#Ülesanne 3
try:
    kommid=int(input("Mitu kommi soovid ära võtta? "))
    #kommide_arv -= kommid
    print(f"Laud on nüüd {kommide_arv} kommi.")
except ValueError:
    print("Palun siseta number ")

kommide_arv=randint(1,100) #int(input("Kommide arv: "))
print(f"Laud on {kommide_arv} kommi.")
kommid=int(input("Mitu kommi soovid ära võtta? "))
kommide_arv -= kommid
print(f"Laud on nüüd {kommide_arv} kommi.")


#Ülesanne 4
ümbermõõt=float(input("Siseta puu ümbermõõt: "))
läbimõõt=round(ümbermõõt/pi,1)
print(f"Puu läbimõõt on {läbimõõt}.")


#Ülesanne5
try:
    pass
except :
    pass
    N=float(input("Siseta ristküliku esimene külg (N): "))
    M=float(input("Siseta ristküliku teine külg (M): "))

    diagonaal=round(sqrt(N**2 + M**2), 1) # sqrt() math moodulist
    print(f"Ristküliku diagonaal on {diagonaal}.") 

#Ülesanne 6
arv1=randint(1, 100)
arv2=randint(1, 100)
arv3=randint(1, 100)
arv4=randint(1, 100)
arv5=randint(1, 100)
summa=arv1+arv2+arv3+arv4+arv5
keskmine=summa/5
print(f"Summa: {summa}, Keskmine: {keskmine}")
try:
    jagaja=int(input("Siseta arv, millega jagada: "))
    tulemus=summa/jagaja
    print("Tulemus:", tulemus) #tulemus=3.75
    print("Täisarvuline osa:", int(tulemus))
    #int(3.75)=3 teine variant täisosa leidiseks on kasutada // jagam
    print("Jääk:", summa % jagaja)
except ValueError:
    print("Palun siseta kehtiv number")



