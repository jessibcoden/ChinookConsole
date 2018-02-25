using System;
using System.Collections.Generic;

namespace ChinookConsole
{
    class View
    {


        string companyName = @"
             _______ _     _                   _
            (_______) |   (_)                 | |    
             _      | |__  _ ____   ___   ___ | |  _ 
            | |     |  _ \| |  _ \ / _ \ / _ \| |_/ )
            | |_____| | | | | | | | |_| | |_| |  _( 
             \______)_| |_|_|_| |_|\___/ \___/|_| \_)
                                         
             _______                       _
            (_______)                     | |       
             _ ___  ____  ___    ___      | | _____ 
            | |     / _ \|  _ \ /___)/ _ \| || ___ |
            | |____| |_| | | | |___ | |_| | || ____|
             \______)___/|_| |_(___/ \___/ \_)_____)";
                                        

        IList<string> _menuItems;
        int itemNumber = 0;

        internal View()
        {
            _menuItems = new List<string> { companyName };
        }

        internal View AddMenuText(string text)
        {
            var menuText = $"{Environment.NewLine}{text}{Environment.NewLine}";
            _menuItems.Add(menuText);
            return this;
        }

        internal View AddMenuOption(string menuItem)
        {
            ++itemNumber;
            var menuEntry = $"{itemNumber}. {menuItem}";
            _menuItems.Add(menuEntry);
            return this;
        }

        internal View AddMenuOptions(IList<string> menuItems)
        {
            foreach (var menuItem in menuItems)
            {
                AddMenuOption(menuItem);
            }

            return this;
        }

        internal string GetFullMenu()
        {
            Console.Clear();
            var menu = string.Join(Environment.NewLine, _menuItems);
            return $"{menu}{Environment.NewLine}> ";
        }
    }
}
