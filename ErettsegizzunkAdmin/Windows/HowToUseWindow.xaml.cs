using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace ErettsegizzunkAdmin.Windows
{
    /// <summary>
    /// Interaction logic for HowToUseWindow.xaml
    /// </summary>
    public partial class HowToUseWindow : Window
    {
        public HowToUseWindow(string honnan)
        {
            InitializeComponent();
            SzovegBeallit(honnan);
        }

        private void SzovegBeallit(string honnan)
        {
            string title = string.Empty;
            
            FlowDocument doc = new FlowDocument();

            switch (honnan)
            {
                case "AdatbazisHelyreallitas":
                    SzovegDesign(doc, "Gombok és használatuk:", "  1. Új mentés készítése: Rákattintva egy friss mentés készül az adatbázisról, a mentés neve tartalmazza a mentés dátumát.");
                    SzovegDesign(doc, string.Empty, "   2. Mentés visszaállítása: A táblázat bal oldalán található kijelölés mezőt jelölje ki annál a biztonsági mentésnél melyet szeretne visszaállítani. Ezek után kattinton a gombra és menjen végig a megjelenő párbeszéd ablakon.");
                    SzovegDesign(doc, string.Empty, "   3. Vissza: Visszatérés a főmenübe");
                    title = "Biztonsági mentés készítése / visszaállítása";
                    break;

                case "EngedelyKezel":
                    SzovegDesign(doc, "Gombok és használatuk:", "   1. Új engedély felvitele: Rákattintva és a felugró ablak utasításait követve új jogosutlságok vehetőek fel.");
                    SzovegDesign(doc, string.Empty, "   2. Összes kijelölése: Rákattintva az összes sor kijelölésre kerül.");
                    SzovegDesign(doc, string.Empty, "   3. Törlés: Rákattintva és a párbeszéd ablakot követve az ÖSSZES kijeölt elem végleges törlésre kerül.");
                    SzovegDesign(doc, string.Empty, "   4. Vissza: Visszatérés a főmenübe");
                    SzovegDesign(doc, string.Empty, "   5. Engedély módosítása: Rákattintva az összes módosítás mentésre kerül.");
                    SzovegDesign(doc, "Engedélyek módosítás1a:", "   Egyes mezőkre duplán kattintva módosítani tudja őket. Ezen módosítások csak azon esetben lesznek elmentve ha rákattint a módosítás elmentés gombra! Egyes mezők biztonsági okokból nem módosíthatóak! (pl. Id)");
                    title = "Jogosultságok kezelése";
                    break;

                case "FeladatokKezel":
                    SzovegDesign(doc, "Gombok és használatuk:", "   1. Új feladatok feltötltése txt: Rákattintva és a felugró ablak utasításait követve új feladatok tölthetőek fel egy tabulátorral tagolt textfile-ból.");
                    SzovegDesign(doc, string.Empty, "   2. Következő oldal: Minend oldalon maximum 50 feladat van megjelenítve. Rákattintva a következő maximum 50 feladat fog megjelenni.");
                    SzovegDesign(doc, string.Empty, "   3. Előző oldal: Minend oldalon maximum 50 feladat van megjelenítve. Rákattintva az előző maximum 50 feladat fog megjelenni.");
                    SzovegDesign(doc, string.Empty, "   4. Összes kijelölése: Rákattintva az összes sor kijelölésre kerül.");
                    SzovegDesign(doc, string.Empty, "   5. Törlés: Rákattintva és a párbeszéd ablakot követve az ÖSSZES kijeölt elem végleges törlésre kerül.");
                    SzovegDesign(doc, string.Empty, "   6. Vissza: Visszatérés a főmenübe");
                    SzovegDesign(doc, string.Empty, "   7. Feladat módosítása: Rákattintva a kijelölt feladat egy felugró ablak keretén belül módosítható.");
                    SzovegDesign(doc, "Feladat módosítása:", "   Jelöljön ki egy feladatot majd kattintson a módosítás gombra. Ezután a felugró ablakban módosíthatja a kiválasztott feladatot.");
                    title = "Feladatok kezelése";
                    break;

                case "FelhasznalokKezel":
                    SzovegDesign(doc, "Gombok és használatuk:", "   1. Új felhasználó felvitele: Rákattintva új felhasználó vihető fel.");
                    SzovegDesign(doc, string.Empty, "   2. Következő oldal: Minend oldalon maximum 50 felhasználó van megjelenítve. Rákattintva a következő maximum 50 felhasználó fog megjelenni.");
                    SzovegDesign(doc, string.Empty, "   3. Előző oldal: Minend oldalon maximum 50 felhasználó van megjelenítve. Rákattintva az előző maximum 50 felhasználó fog megjelenni.");
                    SzovegDesign(doc, string.Empty, "   4. Összes kijelölése: Rákattintva az összes sor kijelölésre kerül.");
                    SzovegDesign(doc, string.Empty, "   5. Felhasználó törlése: Rákattintva és a párbeszéd ablakot követve az ÖSSZES kijeölt elem végleges törlésre kerül.");
                    SzovegDesign(doc, string.Empty, "   6. Vissza: Visszatérés a főmenübe");
                    SzovegDesign(doc, string.Empty, "   7. Módosítások elmentése: Rákattintva az összes módosítás mentésre kerül.");
                    SzovegDesign(doc, "Felhasználók módosítása:", "   Egyes mezőkre duplán kattintva módosítani tudja őket. Ezen módosítások csak azon esetben lesznek elmentve ha rákattint a módosítás elmentés gombra! Egyes mezők biztonsági okokból nem módosíthatóak! (pl. Id)");
                    title = "Felhasználók kezelése";
                    break;

                case "TantargyKezel":
                    SzovegDesign(doc, "Gombok és használatuk:", "   1. Új tantárgy felvitele: Rákattintva új tantárgy vihető fel.");
                    SzovegDesign(doc, string.Empty, "   2. Összes kijelölése: Rákattintva az összes sor kijelölésre kerül.");
                    SzovegDesign(doc, string.Empty, "   3. Törlés: Rákattintva és a párbeszéd ablakot követve az ÖSSZES kijeölt elem végleges törlésre kerül.");
                    SzovegDesign(doc, string.Empty, "   4. Vissza: Visszatérés a főmenübe");
                    SzovegDesign(doc, string.Empty, "   5. Tantárgy módosítása: Rákattintva az összes módosítás mentésre kerül.");
                    SzovegDesign(doc, "Felhasználók módosítása:", "   Egyes mezőkre duplán kattintva módosítani tudja őket. Ezen módosítások csak azon esetben lesznek elmentve ha rákattint a módosítás elmentés gombra! Egyes mezők biztonsági okokból nem módosíthatóak! (pl. Id)");
                    title = "Tantárgyak kezelése";
                    break;

                case "TemaKezel":
                    SzovegDesign(doc, "Gombok és használatuk:", "   1. Új téma felvitele: Rákattintva új téma vihető fel.");
                    SzovegDesign(doc, string.Empty, "   2. Vissza: Visszatérés a főmenübe");
                    SzovegDesign(doc, string.Empty, "   3. Táma módosítása: Rákattintva az összes módosítás mentésre kerül.");
                    SzovegDesign(doc, "Témák módosítása:", "   Egyes mezőkre duplán kattintva módosítani tudja őket. Ezen módosítások csak azon esetben lesznek elmentve ha rákattint a módosítás elmentés gombra! Egyes mezők biztonsági okokból nem módosíthatóak! (pl. Id)");
                    title = "Témák kezelése";
                    break;

            }

            tbTitle.Text = title;
            MainRichTextBox.Document = doc;
        }

        private void SzovegDesign(FlowDocument doc, string title, string szoveg)
        {
            Paragraph paragraph = new Paragraph();

            if (title != string.Empty)
            {
                Bold title2 = new Bold(new Run(title))
                {
                    FontSize = 17,
                    Foreground = Brushes.DarkBlue
                };
                paragraph.Inlines.Add(title2);
                paragraph.Inlines.Add(new LineBreak());
            }

            string[] szovegBontva = szoveg.Split(":");

            if (szovegBontva.Length > 1)
            {
                Bold boldText = new Bold(new Run(szovegBontva[0] + ":"))
                {
                    FontSize = 15,
                    Foreground = Brushes.Black
                };
                paragraph.Inlines.Add(boldText);

                // Add additional text
                Run description = new Run(szovegBontva[1])
                {
                    FontSize = 15
                };
                paragraph.Inlines.Add(description);
            }
            else
            {
                // Add additional text
                Run description = new Run(szovegBontva[0])
                {
                    FontSize = 15
                };
                paragraph.Inlines.Add(description);
            }
            
            doc.Blocks.Add(paragraph);
        }
    }
}