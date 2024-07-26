using FF.Cockpit.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FF.Cockpit.Entity.EntityModel
{
    public class SkinTileContentModel : PropertyChangeHelper
    {
        #region Properties
        private string tile1Text;
        public string Tile1Text
        {
            get { return tile1Text; }
            set { tile1Text = value; OnPropertyChanged(); }
        }
        private string tile2Text;
        public string Tile2Text
        {
            get { return tile2Text; }
            set { tile2Text = value; OnPropertyChanged(); }
        }
        private string tile3Text;
        public string Tile3Text
        {
            get { return tile3Text; }
            set { tile3Text = value; OnPropertyChanged(); }
        }
        private string tile4Text;
        public string Tile4Text
        {
            get { return tile4Text; }
            set { tile4Text = value; OnPropertyChanged(); }
        }
        private string tile5Text;
        public string Tile5Text
        {
            get { return tile5Text; }
            set { tile5Text = value; OnPropertyChanged(); }
        }
        #endregion

        public void OrderSkinTiles(int selectedTile)
        {
            List<int> skintileItems = new List<int>() { 1, 2, 3, 4, 5 };

            if (selectedTile > 0 )
            {
                skintileItems.Remove(selectedTile);
            }
            Tile1Text = selectedTile > 0 ? IntToRomanNumeral(selectedTile) : IntToRomanNumeral(skintileItems[0]);
            Tile2Text = skintileItems.Count == 4? IntToRomanNumeral(skintileItems[0]):  IntToRomanNumeral(skintileItems[1]);
            Tile3Text = skintileItems.Count == 4?IntToRomanNumeral(skintileItems[1]) : IntToRomanNumeral(skintileItems[2]);
            Tile4Text = skintileItems.Count == 4?IntToRomanNumeral(skintileItems[2]) : IntToRomanNumeral(skintileItems[3]);
            Tile5Text = skintileItems.Count == 4?IntToRomanNumeral(skintileItems[3]) : IntToRomanNumeral(skintileItems[4]);
            
        }

        public string IntToRomanNumeral(int number)
        {
            var romanNumeral = string.Empty;
            switch (number)
            {
                case 1: romanNumeral = "I"; break;
                case 2: romanNumeral = "II";break;
                case 3: romanNumeral= "III"; break;
                case 4: romanNumeral = "IV";break;
                case 5: romanNumeral = "V"; break;
            }
            return romanNumeral;
        }
    }
}
