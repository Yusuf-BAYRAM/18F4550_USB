////////////////////////////////////////////////////////////////////////////////
//Programlayan Ugur Yalcin
//21.12.2010
//www.uguryalcin.de
////////////////////////////////////////////////////////////////////////////////

#include <18F4550.h>
#device ADC=10
//#fuses HSPLL,NOWDT,NOPROTECT,NOLVP,NODEBUG,USBDIV,PLL2,CPUDIV1,VREGEN,NOBROWNOUT // 8MHZ
#fuses XTPLL,MCLR,NOWDT,NOPROTECT,NOLVP,NODEBUG,PLL1,CPUDIV1,VREGEN,NOPBADEN //4 MHZ
#use delay(clock=48000000)

#define USB_HID_DEVICE     TRUE             
#define USB_EP1_TX_ENABLE  USB_ENABLE_INTERRUPT  //Uçnokta1'de Kesme transferi aktif
#define USB_EP1_RX_ENABLE  USB_ENABLE_INTERRUPT    
#define USB_EP1_TX_SIZE    64                    //Ucnokta1 için maksimum alinacak ve gonderilecek
#define USB_EP1_RX_SIZE    64                    //veri boyutu (64 byte)

#include "flex_lcd.c"
#include <pic18_usb.h>     
#include <USB_Konfigurasyon.h>                   //USB konfigurasyon bilgileri bu dosyadadir.
#include <usb.c>    
//////////////
#use fast_io(b)
#use fast_io(d)
/////////////
#define use_portb_lcd true
#define low   output_low(PIN_B0)
#define high  output_high(PIN_B0)

/////////////
#define s1_low   output_low(PIN_B0)
#define s1_high  output_high(PIN_B0)

#define s2_low   output_low(PIN_B1)
#define s2_high  output_high(PIN_B1)

#define s3_low   output_low(PIN_B2)
#define s3_high  output_high(PIN_B2)

#define s4_low   output_low(PIN_B3)
#define s4_high  output_high(PIN_B3)

#define s5_low   output_low(PIN_B4)
#define s5_high  output_high(PIN_B4)

#define m1_low   output_low(PIN_B5)
#define m1_high  output_high(PIN_B5)

#define m2_low   output_low(PIN_B6)
#define m2_high  output_high(PIN_B6)

#define UcNokta1       1 
#define kactane        gelen_paket[0]
#define isaret         gelen_paket[1]
#define kontrol        gelen_paket[2]
#define giden          giden_paket[3]
#define servono        gelen_paket[4]
#define adim           gelen_paket[5]
#define ledc           gelen_paket[6]

/////////////
int8 gelen_paket[64];
int8 giden_paket[32]; 
int8 x;
int16 s1,s2,s3,s4,s5;
////////////

void main(void)
{     
   set_tris_b(0x00);
   output_b(0x00);
   set_tris_d(0x00);
   output_d(0x00);
   lcd_init();
   
   
   usb_init();  
   usb_task();  
   printf(lcd_putc,"USB BAGLANTISI");//USB baglantisi kurulduysa LCD'de göster
   lcd_gotoxy(1,2);
   printf(lcd_putc,"KURULUYOR...");//USB baglantisi kurulduysa LCD'de göster
   
   
   usb_wait_for_enumeration();  //Cihaz, hazir olana kadar bekle
   if(usb_enumerated())    
   output_high(pin_d0);
   lcd_gotoxy(1,1);
   printf(lcd_putc,"\fUSB BAGLANTISI");//USB baðlantisi kurulduysa LCD'de göster
   lcd_gotoxy(1,2);
   printf(lcd_putc,"KURULDU !");//USB baglantisi kurulduysa LCD'de göster
     
   for (;;)
   {
      while(usb_enumerated())
      {
  
         if (usb_kbhit(1))             //Eger pc'den yeni bir paket geldiyse
         {         
         usb_get_packet(UcNokta1, gelen_paket, 64); //paketi oku  
           
           
           
           if(ledc=='a')output_high(pin_d1);
           if(ledc=='s')output_high(pin_d2);
           
            switch(ledc)    // Paketin ilk elemanindaki komutu oku ve ilgili göreve git
                {

                case 'a': 
                   output_high(pin_d1);
                break;
                 case 's': 
                   output_high(pin_d2);
                break;
                 case 'd': 
                   output_high(pin_d3);
                break;
                 case 'f': 
                  output_high(pin_d4);
                break;
                 case 'g': 
                   output_high(pin_d5);
                break;
                 case 'h': 
                   output_high(pin_d6);
                break;
                 case 'j': 
                  output_high(pin_d7);
                break;
                
                 case 'z': 
                   output_low(pin_d1);
                break;
                 case 'x': 
                 output_low(pin_d2);
                break;
                 case 'c': 
                 output_low(pin_d3);
                break;
                 case 'v': 
                 output_low(pin_d4);
                break;
                 case 'b': 
               output_low(pin_d5);
                break;
                 case 'n': 
                output_low(pin_d6);
                break;
                 case 'm': 
                 output_low(pin_d7);
                break;
                case 'I'://dc motor 1. ayak
             m1_high;
             m2_low;
             delay_ms(5);
             break;
             case 'G'://dc motor 1. ayak
             m1_low;
             m2_high;
             delay_ms(5);
             break;
             case 'D'://dc motor 1. ayak
             m1_low;
             m2_low;
             delay_ms(5);
             break;
             
        }
           
           
if(isaret == 'Q')
         {
                switch(kontrol)    // Paketin ilk elemanindaki komutu oku ve ilgili göreve git
                {

                case 'a': 
                printf(lcd_putc,"\fPROGRAM BAGLANDI !");//PC deki program baglandiginda LCD'ye yaziliyor...
                break;
                           
                case 's':
                printf(lcd_putc,"\fPROGRAM AYRILDI !");//LCD'ye yaziliyor...
                break; 

              
case 'i': //SERVO 1
///////Veri alindiktan sonra Motorun Dönmesi icin gerekli Formul
      s1=adim*2;
      printf(lcd_putc,"\fGel: %ld",s1);
      s1*=10;
      s1=s1+600;
      lcd_gotoxy(1,2);
      printf(lcd_putc,"\Gon: %ld",s1);
      for(x=0;x<25;x++)
      {
      s1_high;
      delay_us(s1);
      s1_low;
      delay_us(20000-s1);
      }
      break;
case 'j': //SERVO 2
///////Veri alindiktan sonra Motorun Dönmesi icin gerekli Formul
      s2=adim*2;
      printf(lcd_putc,"\fGel: %ld",s2);
      s2*=10;
      s2=s2+600;
      lcd_gotoxy(1,2);
      printf(lcd_putc,"\Gon: %ld",s2);
      for(x=0;x<25;x++)
      {
      s2_high;
      delay_us(s2); 
      s2_low;
      delay_us(20000-s2);
      }
      break;
case 'k': //SERVO 3
///////Veri alindiktan sonra Motorun Dönmesi icin gerekli Formul
      s3=adim*2;
      printf(lcd_putc,"\fGel: %ld",s3);
      s3*=10;
      s3=s3+600;
      lcd_gotoxy(1,2);
      printf(lcd_putc,"\Gon: %ld",s3);
      for(x=0;x<25;x++)
      {
      s3_high;
      delay_us(s3);   
      s3_low;
      delay_us(20000-s3);
      }
      break;
case 'l': //SERVO 4
///////Veri alindiktan sonra Motorun Dönmesi icin gerekli Formul
      s4=adim*2;
      printf(lcd_putc,"\fGel: %ld",s4);
      s4*=10;
      s4=s4+600;
      lcd_gotoxy(1,2);
      printf(lcd_putc,"\Gon: %ld",s4);
      for(x=0;x<25;x++)
      {
      s4_high;
      delay_us(s4);   
      s4_low;
      delay_us(20000-s4);
      }
      break;
case 'm': //SERVO 5
///////Veri alindiktan sonra Motorun Dönmesi icin gerekli Formul
      s5=adim*2;
      printf(lcd_putc,"\fGel: %ld",s5);
      s5*=10;
      s5=s5+600;
      lcd_gotoxy(1,2);
      printf(lcd_putc,"\Gon: %ld",s5);
      for(x=0;x<25;x++)
      {
      s5_high;
      delay_us(s5);  
      s5_low;
      delay_us(20000-s5);
      }
      break;
     
             
                case 'u':
                output_low(pin_d0);
                printf(lcd_putc,"\fLED PASIV !");// LCD'ye yaziliyor...
                giden=0;
                usb_put_packet(1, giden_paket, 64, USB_DTS_TOGGLE); // Geri veri gönderiliyor...
                break;             

                case 'x':
                printf(lcd_putc,"\fBAGLANTI KESILDI !");// LCD'ye yaziliyor...
                break; 
                }
           }
If(isaret=='W')
        {
        lcd_gotoxy(1,1);
        for( x=2;x<=kactane;x++)
        {
         printf(lcd_putc,"%c",gelen_paket[x]);// LCD'ye yaziliyor...
       
        }
        }
        
If(isaret=='Y')
        {
        lcd_gotoxy(1,2);
        for( x=2;x<=kactane;x++)
        {
        printf(lcd_putc,"%c",gelen_paket[x]);// LCD'ye yaziliyor...
       
        }
        }

If(isaret=='Z')
        {
        printf(lcd_putc,"\f");
        }

   }
}
   }
}

