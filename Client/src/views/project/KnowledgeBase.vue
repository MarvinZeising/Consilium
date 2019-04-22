<template>
  <div class="knowledge-base">
    <v-tabs
      v-model="tabsModel"
      color="grey lighten-4"
      align-with-title
    >
      <v-tabs-slider color="primary" />

      <v-tab
        v-for="(tab, i) in tabs"
        :key="i"
      >
        {{tab.name}}
        <!-- // TODO: add popup for when there a too many tabs -->
      </v-tab>

      <NewTabDialog />

      <v-tab-item
        v-for="(tab, i) in tabs"
        :key="i"
      >
        <TabItems v-bind:items="tab.items" />
        <NewTabItemDialog />
      </v-tab-item>
    </v-tabs>
  </div>
</template>

<script lang="ts">
import Vue from 'vue'
import TabItems from '@/components/TabItems.vue'
import NewTabDialog from '@/components/dialogs/NewTabDialog.vue'
import NewTabItemDialog from '@/components/dialogs/NewTabItemDialog.vue'
export default Vue.extend({
  components: { TabItems, NewTabDialog, NewTabItemDialog },
  data() {
    return {
      newTabDialog: null,
      newTabItemDialog: null,
      tabsModel: null,
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
          title: 'Lorem non anim proident aliquip in dolore',
          text: 'Laboris cillum sunt nisi irure commodo sunt exercitation reprehenderit.'
        }, {
          title: 'Id nostrud aliquip irure aliquip voluptate',
          text: 'Cupidatat pariatur laboris do occaecat veniam officia qui sunt laboris dolor amet ut non.'
        }]
      }]
    }
  }
})
</script>
