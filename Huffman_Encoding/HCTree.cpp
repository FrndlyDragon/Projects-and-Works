#include "HCTree.hpp"
#include <iostream>
#include <string>
#include <unordered_map>

void HCTree::build(const vector<int>& freqs) {
    priority_queue<HCNode, vector<HCNode*>, HCNodePtrComp> HCTq;

    //Insert frequencies and leaves into priority queue
    for (int i = 0; i < (int)freqs.size(); i++) {
        if (freqs[i] == 0) {
            continue;
        }
        
        HCNode *temp = new HCNode(freqs[i], i);
        HCTq.push(temp);
    }

    while (!HCTq.empty()) {
        HCNode *node1 = HCTq.top();
        HCTq.pop();
        HCNode *node2 = HCTq.top();
        HCTq.pop();
        HCNode *temp = new HCNode(node1->count + node2->count, 0);

        node1->p = temp;
        node2->p = temp;

        temp->c0 = node1;
        temp->c1 = node2;

        HCTq.push(temp);

        if (node1->c0 == nullptr && node1->c1 == nullptr) {
            leaves[node1->symbol] = node1;
        }

        if (node2->c0 == nullptr && node2->c1 == nullptr) {
            leaves[node2->symbol] = node2;
        }

        if (HCTq.size() == 1) {
            root = HCTq.top();
            HCTq.pop();
            break;
        }
    }
}

void HCTree::encode(unsigned char symbol, unordered_map<char, string> table, FancyOutputStream & out) const {
    auto it = table.find(symbol);
    string temp = it->second;
    int size = temp.length();

    for (int i = 0; i < size; i++) {
        int temp_int = temp[i] - '0';
        out.write_bit(temp_int);
    }
}

unsigned char HCTree::decode(unordered_map<char, string> table, FancyInputStream & in) const {
    string code;
    char ascii;
    int match = 0;

    while (!match) {
        //cout << "finding" << endl;
        int temp = in.read_bit();
        code = code + to_string(temp);
        for (auto it = table.begin(); it != table.end(); it++) {
            if (!code.compare(it->second)) {
                ascii = it->first;
                match = 1;
                break;
            }
        }
    }

    return ascii;
}

HCTree::~HCTree() {
    queue<HCNode*> HCT_delete;
    
    if (root == nullptr) {
        return;
    }

    HCT_delete.push(root);
    HCNode *temp;

    while (!HCT_delete.empty()) {
        temp = HCT_delete.front();
        HCT_delete.pop();

        if (temp->c0 != nullptr) {
           HCT_delete.push(temp->c0); 
        }
        if (temp->c1 != nullptr) {
           HCT_delete.push(temp->c1); 
        }
            
        delete(temp);
    }
}
