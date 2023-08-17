#ifndef HCTREE_HPP
#define HCTREE_HPP
#include <queue>
#include <vector>
#include <fstream>
#include <unordered_map>
#include "Helper.hpp"
using namespace std;

/**
 * A Huffman Code Tree class
 */
class HCTree {
    public:
        vector<HCNode*> leaves;
        HCNode* root;
        /**
         * Constructor, which initializes everything to null pointers
         */
        HCTree() : root(nullptr) {
            leaves = vector<HCNode*>(256, nullptr);
        }

        ~HCTree();

        /**
         * Use the Huffman algorithm to build a Huffman coding tree.
         * PRECONDITION: freqs is a vector of ints, such that freqs[i] is the frequency of occurrence of byte i in the input file.
         * POSTCONDITION: root points to the root of the trie, and leaves[i] points to the leaf node containing byte i.
         */
        void build(const vector<int>& freqs);

        /**
         * Write to the given FancyOutputStream the sequence of bits coding the given symbol.
         * PRECONDITION: build() has been called, to create the coding tree, and initialize root pointer and leaves vector.
         */
        void encode(unsigned char symbol, unordered_map<char, string> table, FancyOutputStream & out) const;

        /**
         * Return symbol coded in the next sequence of bits from the stream.
         * PRECONDITION: build() has been called, to create the coding tree, and initialize root pointer and leaves vector.
         */
        unsigned char decode(unordered_map<char, string> table, FancyInputStream & in) const;
};
#endif // HCTREE_HPP
