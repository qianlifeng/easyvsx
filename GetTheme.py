#!/usr/bin/env python
# -*- coding: UTF-8 -*-
#-------------------------------------------------------------------------------
# Name:        GetTheme.py
# Purpose:
#
# Author:      scott
#
# Created:     18/02/2012
# Copyright:   (c) scott 2012
# Licence:     <your licence>
#-------------------------------------------------------------------------------

from lxml import etree

if __name__ == '__main__':

    vssettingPath = '''C:\Users\scott\Desktop\\light-for-eyes.vssettings'''
    doc = etree.parse(vssettingPath)
    items = doc.xpath("//Item")
    print u"//以下代码有python自动生成"
    for item in items:
        name = item.attrib["Name"]
        foreColor = item.attrib["Foreground"]
        backColor = item.attrib["Background"]
        print 'VSFontColor.SetTextEditColor("%s", %s, %s);' % (name,foreColor,backColor)


