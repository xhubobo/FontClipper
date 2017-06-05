using System;
using System.Collections.Generic;
using System.Text;

using SfntlyTag = com.google.typography.font.sfntly.Tag;
using com.google.typography.font.sfntly;
using com.google.typography.font.sfntly.table.core;
using com.google.typography.font.tools.subsetter;
using com.google.typography.font.tools.sfnttool;
using com.google.typography.font.tools.conversion.woff;
using java.io;
using com.google.typography.font.sfntly.data;
using com.google.typography.font.tools.conversion.eot;
using java.util;
using java.lang;

namespace FontClipper
{
    class SfntlyFontHelper
    {
        private bool strip = false;
        private bool woff = false;
        private bool eot = false;
        private bool mtx = false;

        /// <summary>
        /// 剪辑字体
        /// </summary>
        /// <param name="subString">新字体文本信息</param>
        /// <param name="fontPathOri">原始字体路径</param>
        /// <param name="fontPathNew">新字体路径</param>
        public void ClipFont(string subsetString, string fontPathOri, string fontPathNew)
        {
            try
            {
                var localFileOri = new java.io.File(fontPathOri);
                var localFileNew = new java.io.File(fontPathNew);
                SubsetFontFile(subsetString, localFileOri, localFileNew);
            }
            catch (System.Exception ex)
            {
                throw new System.Exception(ex.Message);
            }
        }

        private void SubsetFontFile(string subsetString, java.io.File paramFile1, java.io.File paramFile2)
        {
            FontFactory localFontFactory = FontFactory.getInstance();
            java.io.FileInputStream localFileInputStream = null;
            try
            {
                localFileInputStream = new java.io.FileInputStream(paramFile1);
                byte[] arrayOfByte = new byte[(int)paramFile1.length()];
                localFileInputStream.read(arrayOfByte);
                Font[] arrayOfFont = null;
                arrayOfFont = localFontFactory.loadFonts(arrayOfByte);
                Font localFont1 = arrayOfFont[0];
                java.util.ArrayList localArrayList = new java.util.ArrayList();
                localArrayList.add(CMapTable.CMapId.WINDOWS_BMP);
                //java.lang.Object localObject1 = null;
                java.lang.Object localObject2 = null;

                Font localFont2 = localFont1;
                java.lang.Object localObject3;
                if (subsetString != null)
                {
                    localObject2 = new RenumberingSubsetter(localFont2, localFontFactory);
                    ((Subsetter)localObject2).setCMaps(localArrayList, 1);
                    localObject3 = (java.lang.Object)GlyphCoverage.getGlyphCoverage(localFont1, subsetString);
                    ((Subsetter)localObject2).setGlyphs((java.util.List)localObject3);
                    var localHashSet = new java.util.HashSet();
                    localHashSet.add(java.lang.Integer.valueOf(SfntlyTag.GDEF));
                    localHashSet.add(java.lang.Integer.valueOf(SfntlyTag.GPOS));
                    localHashSet.add(java.lang.Integer.valueOf(SfntlyTag.GSUB));
                    localHashSet.add(java.lang.Integer.valueOf(SfntlyTag.kern));
                    localHashSet.add(java.lang.Integer.valueOf(SfntlyTag.hdmx));
                    localHashSet.add(java.lang.Integer.valueOf(SfntlyTag.vmtx));
                    localHashSet.add(java.lang.Integer.valueOf(SfntlyTag.VDMX));
                    localHashSet.add(java.lang.Integer.valueOf(SfntlyTag.LTSH));
                    localHashSet.add(java.lang.Integer.valueOf(SfntlyTag.DSIG));
                    localHashSet.add(java.lang.Integer.valueOf(SfntlyTag.intValue(new byte[] { 109, 111, 114, 116 })));
                    localHashSet.add(java.lang.Integer.valueOf(SfntlyTag.intValue(new byte[] { 109, 111, 114, 120 })));
                    ((Subsetter)localObject2).setRemoveTables(localHashSet);
                    localFont2 = ((Subsetter)localObject2).subset().build();
                }
                if (this.strip)
                {
                    localObject2 = new HintStripper(localFont2, localFontFactory);
                    localObject3 = new HashSet();
                    ((Set)localObject3).add(Integer.valueOf(Tag.fpgm));
                    ((Set)localObject3).add(Integer.valueOf(Tag.prep));
                    ((Set)localObject3).add(Integer.valueOf(Tag.cvt));
                    ((Set)localObject3).add(Integer.valueOf(Tag.hdmx));
                    ((Set)localObject3).add(Integer.valueOf(Tag.VDMX));
                    ((Set)localObject3).add(Integer.valueOf(Tag.LTSH));
                    ((Set)localObject3).add(Integer.valueOf(Tag.DSIG));
                    ((Subsetter)localObject2).setRemoveTables((Set)localObject3);
                    localFont2 = ((Subsetter)localObject2).subset().build();
                }
                localObject2 = new java.io.FileOutputStream(paramFile2);
                if (this.woff)
                {
                    localObject3 = new WoffWriter().convert(localFont2);
                    ((WritableFontData)localObject3).copyTo((OutputStream)localObject2);
                }
                else if (this.eot)
                {
                    localObject3 = new EOTWriter(this.mtx).convert(localFont2);
                    ((WritableFontData)localObject3).copyTo((OutputStream)localObject2);
                }
                else
                {
                    localFontFactory.serializeFont(localFont2, (OutputStream)localObject2);
                }
            }
            catch (System.Exception ex)
            {
                throw new System.Exception(ex.Message);
            }
        }
    }
}
