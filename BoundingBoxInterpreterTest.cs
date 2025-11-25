namespace DeepseekOcrExperiments;

[TestClass]
public sealed class BoundingBoxInterpreterTest
{
    readonly string taxiReceiptImage = "/Users/manuelnaujoks/Downloads/IMG_0384.JPEG";
    readonly string taxiReceiptOcrResponse = """
        <|ref|>image<|/ref|><|det|>[[127, 120, 499, 253]]<|/det|>

        <|ref|>text<|/ref|><|det|>[[575, 152, 782, 192]]<|/det|>
        **Fahrpreisquittung**

        <|ref|>text<|/ref|><|det|>[[576, 203, 789, 236]]<|/det|>
        **Tel: 089/23544788**

        <|ref|>text<|/ref|><|det|>[[122, 336, 188, 360]]<|/det|>
        Fahrt am

        <|ref|>text<|/ref|><|det|>[[122, 365, 185, 394]]<|/det|>
        von

        <|ref|>text<|/ref|><|det|>[[122, 394, 561, 444]]<|/det|>
        nach **Heiltsbergmoss Hotel**

        <|ref|>text<|/ref|><|det|>[[123, 440, 362, 479]]<|/det|>
        Flughafenfahrt Stadtfahrt Fernfahrt Kurierfahrt

        <|ref|>image<|/ref|><|det|>[[744, 357, 881, 399]]<|/det|>

        <|ref|>text<|/ref|><|det|>[[123, 483, 234, 515]]<|/det|>
        Sonstiges

        <|ref|>text<|/ref|><|det|>[[619, 476, 725, 500]]<|/det|>
        Zuzahlung nach

        <|ref|>text<|/ref|><|det|>[[620, 509, 703, 532]]<|/det|>
        **§61 SGB(V)**

        <|ref|>text<|/ref|><|det|>[[140, 579, 355, 647]]<|/det|>
        **€** **30,00**

        <|ref|>text<|/ref|><|det|>[[137, 653, 550, 695]]<|/det|>
        Inkl. 7 %MwSt. 19 %MwSt. dankend erhalten

        <|ref|>image<|/ref|><|det|>[[370, 574, 604, 655]]<|/det|>

        <|ref|>text<|/ref|><|det|>[[640, 572, 891, 725]]<|/det|>
        Taxi Unternehmen
        **Ercan Ünalan**
        **Ohmstr.2 85716 Unterschleißheim**
        **089/23544788**
        **info@taxieco/unterschleissheim.de**
        **St. Nr. 1748/163/4005**

        <|ref|>image<|/ref|><|det|>[[137, 703, 604, 790]]<|/det|>

        <|ref|>text<|/ref|><|det|>[[135, 783, 415, 810]]<|/det|>
        Datum Taxi Nr.

        <|ref|>image<|/ref|><|det|>[[725, 690, 860, 760]]<|/det|>
        """;
        
    [TestMethod]
    public async Task UnderstandsBoundingBoxes()
    {
        Assert.Contains("[[127, 120, 499, 253]]", taxiReceiptOcrResponse);
    }
}
