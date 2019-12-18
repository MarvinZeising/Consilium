<template>
  <div>
    <v-flex xs12>
      <h2
        class="headline mb-3"
        v-t="'knowledgeBase.topics'"
      />
    </v-flex>
    <v-flex
      xs12 sm10 md8 lg6
      class="mb-5 pa-2"
    >
      <p>
        {{ $t('knowledgeBase.topicsDescription') }}
      </p>
      <v-card flat>
        <v-list>
          <v-list-tile
            v-for="(topic, index) in getTopics"
            :key="topic.name"
          >
            <v-list-tile-content>
              <v-list-tile-title v-text="topic.name" />
            </v-list-tile-content>

            <v-list-tile-action style="flex-direction:row; align-items:center;">
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
            </v-list-tile-action>
          </v-list-tile>
        </v-list>
      </v-card>
      <v-flex
        xs12
        class="mt-2"
      >
        <NewTopicDialog />
      </v-flex>
    </v-flex>
  </div>
</template>

<script lang="ts">
import Vue from 'vue'
import Component from 'vue-class-component'
import KnowledgeBaseModule from '@/store/modules/knowledgeBase'
import { getModule } from 'vuex-module-decorators'
import { Topic } from '@/models/definitions'
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
export default class Topics extends Vue {
  private knowledgeBaseModule: KnowledgeBaseModule = getModule(KnowledgeBaseModule, this.$store)

  private get getTopics() {
    const projectId = this.$route.params.projectId

    return this.knowledgeBaseModule.allTopics.filter((x: Topic) => x.projectId === projectId)
  }

}
</script>
