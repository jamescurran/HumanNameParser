Name:			HumanNameParser

Date:			12-Nov-2017

Author:		James M. Curran <james.curran@gmail.com> 
          (based on the work of Jason Priem <jason@jasonpriem.com>, et al)

License:		<http://www.opensource.org/licenses/mit-license.php>

Blog entry:  https://honestillusion.com/2017/11/12/human-name-parser


# Description
Port of the original PHP HumanNameParse into C#/.NET (.NET Standard v1.3)
(Original: https://github.com/jasonpriem/HumanNameParser.php )

Takes human names of arbitrary complexity and various wacky formats like:

* J. Walter Weatherman 
* de la Cruz, Ana M. 
* James C. ('Jimmy') O'Dell, Jr.

and parses out the:

* leading initial (Like "J." in "J. Walter Weatherman")
* first name (or first initial in a name like 'R. Crumb')
* nicknames (like "Jimmy" in "James C. ('Jimmy') O'Dell, Jr.")
* middle names
* last name (including compound ones like "van der Sar' and "Ortega y Gasset"), and
* suffix (like 'Jr.', 'III')
