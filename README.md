# Deepseek-OCR Experiments

This experiment demonstrates what is possible with using Deepseek-OCR locally.

## Prerequisites

```bash
ollama serve
ollama pull deepseek-ocr
```

## Original

```prompt
<|grounding|>Convert the document to markdown.
```

![orginal](./taxi_receipt.jpg)

## Annotated

Deepseek-OCR responds with bounding boxes like this:

```output
<|ref|>text<|/ref|><|det|>[[575, 152, 782, 192]]<|/det|>
**Fahrpreisquittung**

<|ref|>text<|/ref|><|det|>[[576, 203, 789, 236]]<|/det|>
**Tel: 089/23544788**

<|ref|>text<|/ref|><|det|>[[122, 336, 188, 360]]<|/det|>
Fahrt am

<|ref|>text<|/ref|><|det|>[[122, 365, 185, 394]]<|/det|>
von
````

![annotated](./taxi_receipt.jpg.annotated.jpg)
