using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[ExcelAsset]
public class DocumentData : ScriptableObject
{
	public List<DocumentEntity> JapaneseDocumentData; // Replace 'EntityType' to an actual type that is serializable.
	public List<DocumentEntity> EnglishDocumentData; // Replace 'EntityType' to an actual type that is serializable.

}
