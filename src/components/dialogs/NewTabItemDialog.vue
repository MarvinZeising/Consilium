<template>
  <v-layout
    row
    justify-center
    class="mt-3"
  >
    <v-dialog
      v-model="newTabItemDialog"
      persistent

    >
      <template v-slot:activator="{ on }">
        <v-btn
          v-on="on"
          color="secondary"
        >
          <v-icon left>add</v-icon>
          Add new item
        </v-btn>
      </template>
      <v-card>
        <v-card-title>
          <span class="headline">Add new tab item</span>
        </v-card-title>
        <v-card-text>
          <v-container grid-list-md>
            <v-layout wrap>
              <v-flex xs12>
                <v-text-field
                  v-model="title"
                  label="Title"
                  required
                  counter
                  maxlength="50"
                  box
                ></v-text-field>
                <v-textarea
                  v-model="content"
                  label="Content"
                  required
                  counter
                  maxlength="2000"
                  max
                  box
                  hint="You can use the MarkDown syntax here."
                  persistent-hint
                ></v-textarea>

                <a
                  href="https://www.markdownguide.org/getting-started"
                  target="_blank"
                  class="caption"
                  style="text-decoration:none"
                >
                  What is MarkDown?
                </a>
                <span> | </span>
                <a
                  href="https://www.markdownguide.org/cheat-sheet"
                  target="_blank"
                  class="caption"
                  style="text-decoration:none"
                >
                  Cheat Sheet
                </a>

                <div
                  v-if="title != '' && content != ''"
                  class="preview-panel"
                >
                  <h3 class="mt-4 mb-3">Preview:</h3>
                  <v-expansion-panel>
                    <v-expansion-panel-content>
                      <template v-slot:header>
                        <div v-text="title" />
                      </template>
                      <v-card>
                        <v-card-text
                          v-html="marked(content)"
                          class="grey lighten-4"
                        />
                      </v-card>
                    </v-expansion-panel-content>
                  </v-expansion-panel>
                </div>
              </v-flex>
            </v-layout>
          </v-container>
        </v-card-text>
        <v-card-actions>
          <v-spacer></v-spacer>
          <v-btn
            flat
            color="black"
            @click="newTabItemDialog = false"
          >
            Close
          </v-btn>
          <v-btn
            :disabled="title == '' || content == ''"
            flat
            color="primary"
            @click="newTabItemDialog = false"
          >
            Save
          </v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>
  </v-layout>
</template>

<script lang="ts">
import Vue from 'vue'
import marked from 'marked'
export default Vue.extend({
  data() {
    return {
      newTabItemDialog: null,
      title: '',
      content: '',
      marked
    }
  }
})
</script>
