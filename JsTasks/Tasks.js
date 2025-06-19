// 1. str satr berilgan. Shu satrdagi undosh harflarni sonini
// qaytaruvchi funksiya tuzing.

// 2. str satr va n soni berilgan. (str.size() > n ) malum bo'lsa
// shu satrni oxiridan n ta belgini qaytaruvchi funksiya tuzing.

// 3. Berilga n soniga muvoffiq sonlardan tashkil topgan shakli ekranga
// chiqaruvchi funksiya tuzing.

// n = 4;                 n = 6
 
// 7 6 5 4                9 8 7 6 5 4
// 7 6 5                  9 8 7 6 5
// 7 6                    9 8 7 6 
// 7                      9 8 7
//                        9 8 
//                        9            


// 4. str satr berilgan. Shu satrdagi unli harflarni o'chirib qaytaruvchi
// funksiya tuzing.

// 5. str satr berilgan. Shu satrdagi barcha raqamlarni "raqam" so'ziga,
// katta harflarni "katta" so'ziga, kichik harflarni "kichik" so'ziga
// almashtirib qaytaruvchi funksiya tuzing.

// 6. str satr berilgan. Shu satrdagi barcha "c++" so'zidan oldin
// "is c++ powerful?" satrni qo'shib qaytaruvchi funksiya tuzing.

// 7. str satr berilgan. Shu satrdagi ohirgi kelgan "qiyin" so'zini
// o'chirib qaytaruvchi funksiya tuzing. Agar ummuman "qiyin" so'zi
// bo'lmasa "PDP" ni qaytarsin

// 8. MxN o'lchamli int tipida massiv berilgan. Shu massivdagi tub elementlarni
// Shu massiv tarkibidagi Eng kichik elementga oshiring va natijani ekranga chiqaring.


// 1. str satr berilgan. Shu satrdagi undosh harflarni sonini
// qaytaruvchi funksiya tuzing.

function countConsonants(str) {
  const consonants = 'bcdfghjklmnpqrstvwxyz';
  let count = 0;

  for (let char of str.toLowerCase()) {
    if (consonants.includes(char)) {
      count++;
    }
  }

  return count;
}

// Example usage:
console.log(countConsonants("bcdfghjklmnpqrstvwxyz"));


// 2. str satr va n soni berilgan. (str.size() > n ) malum bo'lsa
// shu satrni oxiridan n ta belgini qaytaruvchi funksiya tuzing.
function returnNLatterFromStr(str, n){
    let stratIndex = str.length - n;
    if (str.length > n) {
        return str.slice(stratIndex);
    }
    return str;
}

console.log(returnNLatterFromStr("bcdfghjklmnpqrstvwxyz", 3));


// 3. Berilga n soniga muvoffiq sonlardan tashkil topgan shakli ekranga
// chiqaruvchi funksiya tuzing.

// n = 4;                 n = 6
 
// 7 6 5 4                9 8 7 6 5 4
// 7 6 5                  9 8 7 6 5
// 7 6                    9 8 7 6 
// 7                      9 8 7
//                        9 8 
//                        9    

function returnCorespondingNumsToN(n){
    let str = "";
    for (let i = n + n -1; i > n - 1; i--){
        str = str + i;
    }
    return str;
}

function passNumsToMethod(n){
    let str = "";
    for(let i = n; i > 0; i--){
        let nums = returnCorespondingNumsToN(i);
        str = str + nums + "\n";
    }
    return str;
}

console.log(passNumsToMethod(6));


function printShape(n) {
    for (let i = 0; i < n; i++) {
        let line = "";
        for (let j = n + 3; j >= n - i; j--) {
            line += j + " ";
        }
        console.log(line.trim());
    }
}

// Misollar:
printShape(4);
/*
7 6 5 4
7 6 5
7 6
7
*/

console.log("------");

printShape(6);
/*
9 8 7 6 5 4
9 8 7 6 5
9 8 7 6
9 8 7
9 8
9
*/
