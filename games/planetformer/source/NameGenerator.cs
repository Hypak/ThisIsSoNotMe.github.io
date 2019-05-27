using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class NameGenerator {

	public static string GenerateName (int minLength, int maxLength) {
		int length = Random.Range (minLength, maxLength + 1);
		char[] consonants = new char[] {
			'b',
			'c',
			'd',
			'f',
			'g',
			'h',
			'j',
			'k',
			'l',
			'm',
			'n',
			'p',
			'r',
			's',
			't',
			'v',
			'w'
		};
		char[] vowels = new char[] {
			'a',
			'e',
			'i',
			'o',
			'u'
		};
		string name = "";
		int i = 0;
		if (Random.Range (0, 2) == 0) {
			name += vowels [Random.Range (0, vowels.Length)];
			i++;
		}
		for ( ; i < length; i += 2) {
			name += consonants [Random.Range (0, consonants.Length)];
			if (i + 1 < length) {
				name += vowels [Random.Range (0, vowels.Length)];
			}
		}
		name = char.ToUpper (name [0]) + name.Substring (1);
		return name;
	}
}
