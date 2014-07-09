/* 
 * File:   HardwareProfile.h
 * Author: andrew
 *
 * Created on 10 June 2014, 16:40
 */

#ifndef HARDWAREPROFILE_H
#define	HARDWAREPROFILE_H

#ifdef	__cplusplus
extern "C" {
#endif




#ifdef	__cplusplus
}
#endif

#endif	/* HARDWAREPROFILE_H */

// PIC to hardware pin mapping and control macros

#define pickerBus LATB
#define pickerBusTRIS TRISB

#define feederDir LATEbits.LATE3
#define feederStep LATEbits.LATE4
#define feederRelay LATDbits.LATD6
#define feederXHome PORTEbits.RE0
#define feederZHome PORTEbits.RE1

#define feederStepTRIS TRISEbits.TRISE4
#define feederDirTRIS TRISEbits.TRISE3
#define feederRelayTRIS TRISDbits.TRISD6
#define feederXHomeTRIS TRISEbits.TRISE0
#define feederZHomeTRIS TRISEbits.TRISE1


#define baseLED LATDbits.LATD1
#define headLED LATDbits.LATD2
#define baseLEDTRIS TRISDbits.TRISD1
#define headLEDTRIS TRISDbits.TRISD2

#define vibrationPin LATDbits.LATD0
#define vibrationTRIS TRISDbits.TRISD0

#define setVac1on LATDbits.LATD12 = 1; vac1running = 1;
#define setVac1off LATDbits.LATD12 = 0; vac1running = 0;

#define setVac2on LATDbits.LATD4 = 1; vac2running = 1;
#define setVac2off LATDbits.LATD4 = 0; vac2running = 0;

#define setVibrationon LATDbits.LATD0 = 1; vibrationrunning = 1;
#define setVibrationoff LATDbits.LATD0 = 0; vibrationrunning = 0;


 /*******************************************************************/
    /******** USB stack hardware selection options *********************/
    /*******************************************************************/
    //This section is the set of definitions required by the MCHPFSUSB
    //  framework.  These definitions tell the firmware what mode it is
    //  running in, and where it can find the results to some information
    //  that the stack needs.
    //These definitions are required by every application developed with
    //  this revision of the MCHPFSUSB framework.  Please review each
    //  option carefully and determine which options are desired/required
    //  for your application.

    //#define USE_SELF_POWER_SENSE_IO
    #define tris_self_power     TRISAbits.TRISA2    // Input
    #define self_power          1

    //#define USE_USB_BUS_SENSE_IO
    #define tris_usb_bus_sense  TRISBbits.TRISB5    // Input
    #define USB_BUS_SENSE       1

// Device Vendor Indentifier (VID) (0x04D8 is Microchip's VID)
#define USB_VID	0x04D8

// Device Product Indentifier (PID) (0x0042)
#define USB_PID	0x0042

// Manufacturer string descriptor
#define MSDLENGTH	17
#define MSD		'A','B',' ','E','l','e','c','t','r','o','n','i','c','s',' ','U','K'

// Product String descriptor
#define PSDLENGTH	25
#define PSD		'P','i','c','k',' ','a','n','d',' ','P','l','a','c','e',' ','C','o','n','t','r','o','l','l','e','r'

// Device serial number string descriptor
#define DSNLENGTH	7
#define DSN		'V','e','r','_','3','.','0'