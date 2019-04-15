<template>
  <div class="knowledge-base">
    <v-tabs
      color="grey lighten-4"
      align-with-title
    >
      <v-tabs-slider color="primary" />

      <v-tab
        v-for="tab in tabs"
        :key="tab.name + ': Header'"
      >
        {{ tab.name }}
      </v-tab>
    </v-tabs>

    <v-tabs-items>
      <v-tab-item
        v-for="tab in tabs"
        :key="tab.name + ': Content'"
      >
        <div class="ma-3">
          <v-expansion-panel
            :expand=true
          >
            <v-expansion-panel-content
              v-for="item in tab.items"
              :key="tab.name + ': ' + item.title"
            >
              <template v-slot:header>
                <div v-text="item.title" />
              </template>
              <v-card>
                <v-card-text
                  v-text="item.text"
                  class="grey lighten-4"
                />
                <v-card-actions>
                  <v-spacer></v-spacer>
                  <v-btn flat color="secondary">
                    <v-icon class="mr-2">arrow_upward</v-icon>
                    Move up
                  </v-btn>
                  <v-btn flat color="secondary">
                    <v-icon class="mr-2">arrow_downward</v-icon>
                    Move down
                  </v-btn>
                  <v-btn flat color="primary">
                    <v-icon class="mr-2">edit</v-icon>
                    Edit
                  </v-btn>
                </v-card-actions>
              </v-card>
            </v-expansion-panel-content>
          </v-expansion-panel>

          <div class="text-xs-center mt-3">
            <v-btn>
              <v-icon left>add</v-icon>
              Add new item
            </v-btn>
          </div>
        </div>
      </v-tab-item>
    </v-tabs-items>
  </div>
</template>

<script lang="ts">
import Vue from 'vue'
export default Vue.extend({
  data () {
    return {
      tab: null,
      panel: null,
      tabs: [{
        name: 'FAQs',
        items: [{
          title: 'Et mollit quis duis velit',
          text: 'Cupidatat dolor ex cupidatat minim do amet ex ullamco.'
        }]
      }, {
        name: 'About the Team',
        items: [{
          title: 'Est minim ad cupidatat anim',
          text: 'Esse irure culpa incididunt cupidatat consequat id.'
        }]
      }, {
        name: 'Meeting-Point Information',
        items: [{
          title: 'Non ipsum velit consequat exercitation ut duis',
          text: 'Ex ex ullamco sint do ullamco ipsum ex ex.'
        }]
      }, {
        name: 'Videos',
        items: [{
          title: 'Lorem proident culpa cillum ad',
          text: 'Aliqua ullamco pariatur proident labore incididunt.'
        }]
      }, {
        name: 'Miscellaneous',
        items: [{
          title: 'Laborum irure do exercitation et quis',
          text: 'Aute velit irure aliqua fugiat ut dolore elit officia in exercitation dolor.'
        }, {
          title: 'Id nostrud aliquip irure aliquip voluptate',
          text: 'Cupidatat pariatur laboris do occaecat veniam officia qui sunt laboris dolor amet ut non.'
        }]
      }]
    }
  }
})
</script>
