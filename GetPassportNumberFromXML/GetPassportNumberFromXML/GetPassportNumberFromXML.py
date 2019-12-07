from lxml import  etree

def ParseXML(xmlFile, outputFileName):
    with open(xmlFile) as fobj:
        xml = fobj.read()

    root = etree.fromstring(xml)
    numbers = []
    for appt in root.getchildren():
        for elem in appt.getchildren():
            if elem.tag == '{http://schemas.xmlsoap.org/soap/envelope/}BoxShipmentNotificationA':
                for box in elem.getchildren():
                    if box.tag =='{http://xsd.gspvd/v001/personalization/pc/dpc-notifications}DocumentEntry':
                        for passport in box.getchildren():
                            if passport.tag == '{http://xsd.gspvd/v001/personalization/pc/dpc-notifications}DocumentNumber':
                                if not passport.text:
                                    text = "None"
                                else:
                                    text = passport.text
                                numbers.append(text)
                                print(passport.tag + " => " + text)
    
    outputFile = open(outputFileName,'w')

    for item in numbers:
        out = '\''+item+'\','
        outputFile.write("%s\n" % out)


pathToXML = "C:\\Users\\zavodkin_r_s\\Desktop\\3535MTG.xml"
pathToOutputFile ="C:\\Users\\zavodkin_r_s\\Desktop\\right numbers.txt"
ParseXML(pathToXML,pathToOutputFile)
