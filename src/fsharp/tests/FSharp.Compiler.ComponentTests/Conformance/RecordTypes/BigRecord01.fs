// #Regression #Conformance #TypesAndModules #Records 
// Regression for 6346, used to stackoverflow with this many fields (still do with 300+)

type BigRecord = {
    field1 : int
    field2 : int
    field3 : int
    field4 : int
    field5 : int
    field6 : int
    field7 : int
    field8 : int
    field9 : int
    field10 : int
    field11 : int
    field12 : int
    field13 : int
    field14 : int
    field15 : int
    field16 : int
    field17 : int
    field18 : int
    field19 : int
    field20 : int
    field21 : int
    field22 : int
    field23 : int
    field24 : int
    field25 : int
    field26 : int
    field27 : int
    field28 : int
    field29 : int
    field30 : int
    field31 : int
    field32 : int
    field33 : int
    field34 : int
    field35 : int
    field36 : int
    field37 : int
    field38 : int
    field39 : int
    field40 : int
    field41 : int
    field42 : int
    field43 : int
    field44 : int
    field45 : int
    field46 : int
    field47 : int
    field48 : int
    field49 : int
    field50 : int
    field51 : int
    field52 : int
    field53 : int
    field54 : int
    field55 : int
    field56 : int
    field57 : int
    field58 : int
    field59 : int
    field60 : int
    field61 : int
    field62 : int
    field63 : int
    field64 : int
    field65 : int
    field66 : int
    field67 : int
    field68 : int
    field69 : int
    field70 : int
    field71 : int
    field72 : int
    field73 : int
    field74 : int
    field75 : int
    field76 : int
    field77 : int
    field78 : int
    field79 : int
    field80 : int
    field81 : int
    field82 : int
    field83 : int
    field84 : int
    field85 : int
    field86 : int
    field87 : int
    field88 : int
    field89 : int
    field90 : int
    field91 : int
    field92 : int
    field93 : int
    field94 : int
    field95 : int
    field96 : int
    field97 : int
    field98 : int
    field99 : int
    field100 : int
    field101 : int
    field102 : int
    field103 : int
    field104 : int
    field105 : int
    field106 : int
    field107 : int
    field108 : int
    field109 : int
    field110 : int
    field111 : int
    field112 : int
    field113 : int
    field114 : int
    field115 : int
    field116 : int
    field117 : int
    field118 : int
    field119 : int
    field120 : int
    field121 : int
    field122 : int
    field123 : int
    field124 : int
    field125 : int
    field126 : int
    field127 : int
    field128 : int
    field129 : int
    field130 : int
    field131 : int
    field132 : int
    field133 : int
    field134 : int
    field135 : int
    field136 : int
    field137 : int
    field138 : int
    field139 : int
    field140 : int
    field141 : int
    field142 : int
    field143 : int
    field144 : int
    field145 : int
    field146 : int
    field147 : int
    field148 : int
    field149 : int
    field150 : int
    field151 : int
    field152 : int
    field153 : int
    field154 : int
    field155 : int
    field156 : int
    field157 : int
    field158 : int
    field159 : int
    field160 : int
    field161 : int
    field162 : int
    field163 : int
    field164 : int
    field165 : int
    field166 : int
    field167 : int
    field168 : int
    field169 : int
    field170 : int
    field171 : int
    field172 : int
    field173 : int
    field174 : int
    field175 : int
    field176 : int
    field177 : int
    field178 : int
    field179 : int
    field180 : int
    field181 : int
    field182 : int
    field183 : int
    field184 : int
    field185 : int
    field186 : int
    field187 : int
    field188 : int
    field189 : int
    field190 : int
    field191 : int
    field192 : int
    field193 : int
    field194 : int
    field195 : int
    field196 : int
    field197 : int
    field198 : int
    field199 : int
    field200 : int
    field201 : int
    field202 : int
    field203 : int
    field204 : int
    field205 : int
    field206 : int
    field207 : int
    field208 : int
    field209 : int
    field210 : int
    field211 : int
    field212 : int
    field213 : int
    field214 : int
    field215 : int
    field216 : int
    field217 : int
    field218 : int
    field219 : int
    field220 : int
    field221 : int
    field222 : int
    field223 : int
    field224 : int
    field225 : int
    field226 : int
    field227 : int
    field228 : int
    field229 : int
    field230 : int
    field231 : int
    field232 : int
    field233 : int
    field234 : int
    field235 : int
    field236 : int
    field237 : int
    field238 : int
    field239 : int
    field240 : int
    field241 : int
    field242 : int
    field243 : int
    field244 : int
    field245 : int
    field246 : int
    field247 : int
    field248 : int
    field249 : int
    field250 : int
    }
let CloneRec(old:BigRecord) =
    {
        field1 = old.field1
        field2 = old.field2
        field3 = old.field3
        field4 = old.field4
        field5 = old.field5
        field6 = old.field6
        field7 = old.field7
        field8 = old.field8
        field9 = old.field9
        field10 = old.field10
        field11 = old.field11
        field12 = old.field12
        field13 = old.field13
        field14 = old.field14
        field15 = old.field15
        field16 = old.field16
        field17 = old.field17
        field18 = old.field18
        field19 = old.field19
        field20 = old.field20
        field21 = old.field21
        field22 = old.field22
        field23 = old.field23
        field24 = old.field24
        field25 = old.field25
        field26 = old.field26
        field27 = old.field27
        field28 = old.field28
        field29 = old.field29
        field30 = old.field30
        field31 = old.field31
        field32 = old.field32
        field33 = old.field33
        field34 = old.field34
        field35 = old.field35
        field36 = old.field36
        field37 = old.field37
        field38 = old.field38
        field39 = old.field39
        field40 = old.field40
        field41 = old.field41
        field42 = old.field42
        field43 = old.field43
        field44 = old.field44
        field45 = old.field45
        field46 = old.field46
        field47 = old.field47
        field48 = old.field48
        field49 = old.field49
        field50 = old.field50
        field51 = old.field51
        field52 = old.field52
        field53 = old.field53
        field54 = old.field54
        field55 = old.field55
        field56 = old.field56
        field57 = old.field57
        field58 = old.field58
        field59 = old.field59
        field60 = old.field60
        field61 = old.field61
        field62 = old.field62
        field63 = old.field63
        field64 = old.field64
        field65 = old.field65
        field66 = old.field66
        field67 = old.field67
        field68 = old.field68
        field69 = old.field69
        field70 = old.field70
        field71 = old.field71
        field72 = old.field72
        field73 = old.field73
        field74 = old.field74
        field75 = old.field75
        field76 = old.field76
        field77 = old.field77
        field78 = old.field78
        field79 = old.field79
        field80 = old.field80
        field81 = old.field81
        field82 = old.field82
        field83 = old.field83
        field84 = old.field84
        field85 = old.field85
        field86 = old.field86
        field87 = old.field87
        field88 = old.field88
        field89 = old.field89
        field90 = old.field90
        field91 = old.field91
        field92 = old.field92
        field93 = old.field93
        field94 = old.field94
        field95 = old.field95
        field96 = old.field96
        field97 = old.field97
        field98 = old.field98
        field99 = old.field99
        field100 = old.field100
        field101 = old.field101
        field102 = old.field102
        field103 = old.field103
        field104 = old.field104
        field105 = old.field105
        field106 = old.field106
        field107 = old.field107
        field108 = old.field108
        field109 = old.field109
        field110 = old.field110
        field111 = old.field111
        field112 = old.field112
        field113 = old.field113
        field114 = old.field114
        field115 = old.field115
        field116 = old.field116
        field117 = old.field117
        field118 = old.field118
        field119 = old.field119
        field120 = old.field120
        field121 = old.field121
        field122 = old.field122
        field123 = old.field123
        field124 = old.field124
        field125 = old.field125
        field126 = old.field126
        field127 = old.field127
        field128 = old.field128
        field129 = old.field129
        field130 = old.field130
        field131 = old.field131
        field132 = old.field132
        field133 = old.field133
        field134 = old.field134
        field135 = old.field135
        field136 = old.field136
        field137 = old.field137
        field138 = old.field138
        field139 = old.field139
        field140 = old.field140
        field141 = old.field141
        field142 = old.field142
        field143 = old.field143
        field144 = old.field144
        field145 = old.field145
        field146 = old.field146
        field147 = old.field147
        field148 = old.field148
        field149 = old.field149
        field150 = old.field150
        field151 = old.field151
        field152 = old.field152
        field153 = old.field153
        field154 = old.field154
        field155 = old.field155
        field156 = old.field156
        field157 = old.field157
        field158 = old.field158
        field159 = old.field159
        field160 = old.field160
        field161 = old.field161
        field162 = old.field162
        field163 = old.field163
        field164 = old.field164
        field165 = old.field165
        field166 = old.field166
        field167 = old.field167
        field168 = old.field168
        field169 = old.field169
        field170 = old.field170
        field171 = old.field171
        field172 = old.field172
        field173 = old.field173
        field174 = old.field174
        field175 = old.field175
        field176 = old.field176
        field177 = old.field177
        field178 = old.field178
        field179 = old.field179
        field180 = old.field180
        field181 = old.field181
        field182 = old.field182
        field183 = old.field183
        field184 = old.field184
        field185 = old.field185
        field186 = old.field186
        field187 = old.field187
        field188 = old.field188
        field189 = old.field189
        field190 = old.field190
        field191 = old.field191
        field192 = old.field192
        field193 = old.field193
        field194 = old.field194
        field195 = old.field195
        field196 = old.field196
        field197 = old.field197
        field198 = old.field198
        field199 = old.field199
        field200 = old.field200
        field201 = old.field201
        field202 = old.field202
        field203 = old.field203
        field204 = old.field204
        field205 = old.field205
        field206 = old.field206
        field207 = old.field207
        field208 = old.field208
        field209 = old.field209
        field210 = old.field210
        field211 = old.field211
        field212 = old.field212
        field213 = old.field213
        field214 = old.field214
        field215 = old.field215
        field216 = old.field216
        field217 = old.field217
        field218 = old.field218
        field219 = old.field219
        field220 = old.field220
        field221 = old.field221
        field222 = old.field222
        field223 = old.field223
        field224 = old.field224
        field225 = old.field225
        field226 = old.field226
        field227 = old.field227
        field228 = old.field228
        field229 = old.field229
        field230 = old.field230
        field231 = old.field231
        field232 = old.field232
        field233 = old.field233
        field234 = old.field234
        field235 = old.field235
        field236 = old.field236
        field237 = old.field237
        field238 = old.field238
        field239 = old.field239
        field240 = old.field240
        field241 = old.field241
        field242 = old.field242
        field243 = old.field243
        field244 = old.field244
        field245 = old.field245
        field246 = old.field246
        field247 = old.field247
        field248 = old.field248
        field249 = old.field249
        field250 = old.field250
    }
let x = {
        field1 = 5;
        field2 = 5;
        field3 = 5;
        field4 = 5;
        field5 = 5;
        field6 = 5;
        field7 = 5;
        field8 = 5;
        field9 = 5;
        field10 = 5;
        field11 = 5;
        field12 = 5;
        field13 = 5;
        field14 = 5;
        field15 = 5;
        field16 = 5;
        field17 = 5;
        field18 = 5;
        field19 = 5;
        field20 = 5;
        field21 = 5;
        field22 = 5;
        field23 = 5;
        field24 = 5;
        field25 = 5;
        field26 = 5;
        field27 = 5;
        field28 = 5;
        field29 = 5;
        field30 = 5;
        field31 = 5;
        field32 = 5;
        field33 = 5;
        field34 = 5;
        field35 = 5;
        field36 = 5;
        field37 = 5;
        field38 = 5;
        field39 = 5;
        field40 = 5;
        field41 = 5;
        field42 = 5;
        field43 = 5;
        field44 = 5;
        field45 = 5;
        field46 = 5;
        field47 = 5;
        field48 = 5;
        field49 = 5;
        field50 = 5;
        field51 = 5;
        field52 = 5;
        field53 = 5;
        field54 = 5;
        field55 = 5;
        field56 = 5;
        field57 = 5;
        field58 = 5;
        field59 = 5;
        field60 = 5;
        field61 = 5;
        field62 = 5;
        field63 = 5;
        field64 = 5;
        field65 = 5;
        field66 = 5;
        field67 = 5;
        field68 = 5;
        field69 = 5;
        field70 = 5;
        field71 = 5;
        field72 = 5;
        field73 = 5;
        field74 = 5;
        field75 = 5;
        field76 = 5;
        field77 = 5;
        field78 = 5;
        field79 = 5;
        field80 = 5;
        field81 = 5;
        field82 = 5;
        field83 = 5;
        field84 = 5;
        field85 = 5;
        field86 = 5;
        field87 = 5;
        field88 = 5;
        field89 = 5;
        field90 = 5;
        field91 = 5;
        field92 = 5;
        field93 = 5;
        field94 = 5;
        field95 = 5;
        field96 = 5;
        field97 = 5;
        field98 = 5;
        field99 = 5;
        field100 = 5;
        field101 = 5;
        field102 = 5;
        field103 = 5;
        field104 = 5;
        field105 = 5;
        field106 = 5;
        field107 = 5;
        field108 = 5;
        field109 = 5;
        field110 = 5;
        field111 = 5;
        field112 = 5;
        field113 = 5;
        field114 = 5;
        field115 = 5;
        field116 = 5;
        field117 = 5;
        field118 = 5;
        field119 = 5;
        field120 = 5;
        field121 = 5;
        field122 = 5;
        field123 = 5;
        field124 = 5;
        field125 = 5;
        field126 = 5;
        field127 = 5;
        field128 = 5;
        field129 = 5;
        field130 = 5;
        field131 = 5;
        field132 = 5;
        field133 = 5;
        field134 = 5;
        field135 = 5;
        field136 = 5;
        field137 = 5;
        field138 = 5;
        field139 = 5;
        field140 = 5;
        field141 = 5;
        field142 = 5;
        field143 = 5;
        field144 = 5;
        field145 = 5;
        field146 = 5;
        field147 = 5;
        field148 = 5;
        field149 = 5;
        field150 = 5;
        field151 = 5;
        field152 = 5;
        field153 = 5;
        field154 = 5;
        field155 = 5;
        field156 = 5;
        field157 = 5;
        field158 = 5;
        field159 = 5;
        field160 = 5;
        field161 = 5;
        field162 = 5;
        field163 = 5;
        field164 = 5;
        field165 = 5;
        field166 = 5;
        field167 = 5;
        field168 = 5;
        field169 = 5;
        field170 = 5;
        field171 = 5;
        field172 = 5;
        field173 = 5;
        field174 = 5;
        field175 = 5;
        field176 = 5;
        field177 = 5;
        field178 = 5;
        field179 = 5;
        field180 = 5;
        field181 = 5;
        field182 = 5;
        field183 = 5;
        field184 = 5;
        field185 = 5;
        field186 = 5;
        field187 = 5;
        field188 = 5;
        field189 = 5;
        field190 = 5;
        field191 = 5;
        field192 = 5;
        field193 = 5;
        field194 = 5;
        field195 = 5;
        field196 = 5;
        field197 = 5;
        field198 = 5;
        field199 = 5;
        field200 = 5;
        field201 = 5;
        field202 = 5;
        field203 = 5;
        field204 = 5;
        field205 = 5;
        field206 = 5;
        field207 = 5;
        field208 = 5;
        field209 = 5;
        field210 = 5;
        field211 = 5;
        field212 = 5;
        field213 = 5;
        field214 = 5;
        field215 = 5;
        field216 = 5;
        field217 = 5;
        field218 = 5;
        field219 = 5;
        field220 = 5;
        field221 = 5;
        field222 = 5;
        field223 = 5;
        field224 = 5;
        field225 = 5;
        field226 = 5;
        field227 = 5;
        field228 = 5;
        field229 = 5;
        field230 = 5;
        field231 = 5;
        field232 = 5;
        field233 = 5;
        field234 = 5;
        field235 = 5;
        field236 = 5;
        field237 = 5;
        field238 = 5;
        field239 = 5;
        field240 = 5;
        field241 = 5;
        field242 = 5;
        field243 = 5;
        field244 = 5;
        field245 = 5;
        field246 = 5;
        field247 = 5;
        field248 = 5;
        field249 = 5;
        field250 = 5;
}

let y = CloneRec(x)
