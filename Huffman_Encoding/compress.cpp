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

map<char, int> construct_HCTMap(HCTree *tree) {
    map<char, int> HCTMap;
    
    for (int i = 0; i < (int)tree->leaves.size(); i++) {
        if (tree->leaves[i] == nullptr) {
            continue;
        }

        HCNode *temp = tree->leaves[i];

        int codeLength = 0;
        int codeChar = temp->symbol;

        while (temp != tree->root) {
            temp = temp->p;
            codeLength++;
        }

        HCTMap.insert(pair<char, int>(codeChar, codeLength));
    }

    return HCTMap;
}

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
    int charCount = 0;
    vector<int> freq(256, 0);
    HCTree *tree = new HCTree();

    FancyInputStream *fileIn = new FancyInputStream(argv[1]);
    FancyOutputStream *fileOut = new FancyOutputStream(argv[2]);
    
    if (fileIn->filesize() == 0) {
        delete(fileIn);
        delete(fileOut);
        delete(tree);
        return 0;
    }

    unsigned char ch = fileIn->read_byte();
    while (fileIn->good()) {
        int temp = freq[ch];
        freq[ch] = temp + 1;
        charCount++;
        ch = fileIn->read_byte();
    }

    tree->build(freq);

    map<char, int> HCTMap = construct_HCTMap(tree);
    set<pair<char, int>, compare_length> sorted_HCC(HCTMap.begin(), HCTMap.end());
    unordered_map<char, string> HCC_table = HCC_generator(sorted_HCC);

    fileIn->reset();

    //Header encoding
    fileOut->write_int(charCount);

    for (int i = 0; i < 256; i++) {
        char tempCh = i;
        auto it = HCC_table.find(tempCh);
        if (it == HCC_table.end()) {
            fileOut->write_byte(0);
        }
        else {
            tempCh = it->second.length();
            fileOut->write_byte(tempCh);
        }
    }

    //encode file
    ch = fileIn->read_byte();
    while (fileIn->good()) {
        tree->encode(ch, HCC_table, *fileOut);
        ch = fileIn->read_byte();
    }

    delete(fileIn);
    delete(fileOut);
    delete(tree);
    return 0;
}
