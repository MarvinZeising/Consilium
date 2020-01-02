<template>
  <v-flex xs12 sm10 md8 lg6 xl4>
    <h2
      class="headline mb-3"
      v-t="'knowledgeBase.topics'"
    />
    <v-card
      flat
      class="ma-2 mb-5"
    >
      <v-card-text
        class="grey--text"
        v-t="'knowledgeBase.topicsDescription'"
      />
      <v-list>
        <v-list-item v-if="getTopics.length === 0">
          <span v-t="'knowledgeBase.noTopics'" />
        </v-list-item>
        <v-list-item
          v-for="(topic, index) in getTopics"
          :key="topic.name"
        >
          <v-list-item-content>
            <v-list-item-title v-text="topic.name" />
          </v-list-item-content>

          <v-list-item-action style="flex-direction:row; align-items:center;">
            <v-btn
              v-if="index > 0"
              icon
              class="ma-0"
              @click="knowledgeBaseModule.moveTopicUp(topic.order)"
            >
              <v-icon color="grey">expand_less</v-icon>
            </v-btn>
            <v-btn
              v-if="index < getTopics.length - 1"
              icon
              class="ma-0"
              @click="knowledgeBaseModule.moveTopicDown(topic.order)"
            >
              <v-icon color="grey">expand_more</v-icon>
            </v-btn>
            <RenameTopicDialog :topicId="topic.id" />
            <DeleteTopicDialog :topicId="topic.id" />
          </v-list-item-action>
        </v-list-item>
      </v-list>
      <v-card-actions>
        <v-spacer />
        <NewTopicDialog />
      </v-card-actions>
    </v-card>
  </v-flex>
</template>

<script lang="ts">
import { Vue, Component } from 'vue-property-decorator'
import { getModule } from 'vuex-module-decorators'
import KnowledgeBaseModule from '../store/knowledgeBase'
import { Topic } from '../models/definitions'
import NewTopicDialog from './dialogs/NewTopicDialog.vue'
import RenameTopicDialog from './dialogs/RenameTopicDialog.vue'
import DeleteTopicDialog from './dialogs/DeleteTopicDialog.vue'

@Component({
  components: {
    NewTopicDialog,
    RenameTopicDialog,
    DeleteTopicDialog,
  }
})
export default class SettingsTopics extends Vue {
  private knowledgeBaseModule: KnowledgeBaseModule = getModule(KnowledgeBaseModule, this.$store)

  private get getTopics() {
    const projectId = this.$route.params.projectId

    return this.knowledgeBaseModule.allTopics.filter((x: Topic) => x.projectId === projectId)
  }

}
</script>
