   M S B A M L   `   `   `  ����F  BEditWeapons, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null)clr-namespace:EditWeaponsEditWeapons  5   	       EditWeapons.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   +localclr-namespace:EditWeapons     5   	     � Title$  
MainWindow��5   	   $��379.292��6   $��205.188��6-   .��5	      �  � RowDefinitions 5
   
   �� 5      .+�5   
   �� -   5         ��Name$ Primary��5      $��23��6#   $��
10,26,10,0q�6/    �� TextWrapping$
 Wrap=�6C   $��mp_weapon_rspn101��6W   $	��Top=�6p   �� 5   
   -   5        $ 	Secondary��5      $��23��6%   $��
10,86,10,0q�61   $
 Wrap=�6E   $��mp_weapon_semipistol��6Y   $	��Top=�6u   �� 5   
   -   5        $ Ordnance��5      $��23��6$   $��10,149,10,0q�60   $
 Wrap=�6E   $��mp_weapon_frag_grenade��6Y   $	��Top=�6w   �� 5   
   -   5        $ Special��5      $��23��6#   $��10,208,10,0q�6/   $
 Wrap=�6D   $��mp_ability_cloak��6X   $	��Top=�6p   �� 5   
   -   5         ��Name$ 	textBlock��5      $��10,10,128,0q�6'   $
��Wrap=�6<   $��Primary ��6P   $	��Top=�5      �� 5   
   -   5        $ textBlock_Copy��5      $��10,71,114,0q�6,   $
��Wrap=�6A   $��
Secondary ��6U   $	��Top=�6g   �� 5   
   -   5        $ textBlock_Copy1��5      $��10,133,117,0q�6-   $
��Wrap=�6C   $��	Ordnance ��6W   $	��Top=�6h   �� 5   
   -   5        $ textBlock_Copy2��5      $��10,192,132,0q�6-   $
��Wrap=�6C   $��Special ��6W   $	��Top=�6g   �� 5   
   -	   5         ��Name$ Write��5      $��Change��6    $��
10,0,10,10q�61   $��Bottom=�6E   6`   �� 5   
   -
   5        $ Melee��5      $��23��6!   $��10,266,10,0q�6-   $
 Wrap=�6B   $��melee_pilot_emptyhanded��6V   $	��Top=�6u   �� 5   
   $��10,250,132,0q�6   $
��Wrap=�6*   $��Melee��6>   $	��Top=�6K   5      5      