#include <stdio.h>
#include <map>
#include <string>
#include <unordered_map>
#include <set>
#include <bitset>
#include "Helper.hpp"
#include "HCTree.hpp"

struct compare_length {
    bool operator()(const pair<char, int> &a, const pair<char, int> &b) {
        if (a.second < b.second) {
            return true;
        }
        else if (a.second == b.second){
            if (a.first < b.first) {
                return true;
            }
            return false;
        }
        else {
            return false;
        }
    }
};

unordered_map<char, string> HCC_generator(set<pair<char, int>, compare_length> sorted_HCC) {
    unordered_map<char, string> HCC_table;

    auto it = next(sorted_HCC.begin(), 0);
    int hcc_code = 0;
    int length_1 = 0;
    int length_2 = it->second;

    hcc_code = hcc_code << (length_2 - length_1);
    string code = bitset<32>(hcc_code).to_string().substr(32 - length_2, 32);
    HCC_table.insert({it->first, code});

    length_1 = length_2;

    for (int i = 1; i < (int)sorted_HCC.size(); i++) {
        it = next(sorted_HCC.begin(), i);
        length_2 = it->second;

        hcc_code = (hcc_code + 1) << (length_2 - length_1);
        string code = bitset<32>(hcc_code).to_string().substr(32 - length_2, 32);
        HCC_table.insert({it->first, code});

        length_1 = length_2;
    }

    return HCC_table;
}

int main(int argc, char* argv[]) {
    FancyInputStream *fileIn = new FancyInputStream(argv[1]);
    FancyOutputStream *fileOut = new FancyOutputStream(argv[2]);
    HCTree *tree = new HCTree();
    map<char, int> HCT_map;

    if (fileIn->filesize() == 0) {
        delete(fileIn);
        delete(fileOut);
        delete(tree);
        return 0;
    }

    int charCount = fileIn->read_int();

    for (int i = 0; i < 256; i++) {
        int temp = fileIn->read_byte();
        if (temp == 0) {
            continue;
        }

        char ch = i;
        //cout << "Adding: " << ch << endl;
        HCT_map.insert(pair<char, int>(ch, temp));
    }

    set<pair<char, int>, compare_length> sorted_HCC(HCT_map.begin(), HCT_map.end());
    unordered_map<char, string> HCC_table = HCC_generator(sorted_HCC);

    for (int j = 0; j < charCount; j++) {
        char out = tree->decode(HCC_table, *fileIn);
        fileOut->write_byte(out);
     }

    delete(tree);
    delete(fileIn);
    delete(fileOut);
    return 0;
}
