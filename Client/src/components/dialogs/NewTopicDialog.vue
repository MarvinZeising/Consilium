<template>
  <v-layout>
    <v-dialog v-model="newTopicDialog" persistent max-width="600px">
      <template v-slot:activator="{ on }">
        <v-btn
          v-on="on"
          color="primary"
          v-t="'knowledgeBase.createTopic'"
        />
      </template>
      <v-card>
        <v-form v-model="form">
          <v-card-title>
            <span class="headline">Add new Topic</span>
          </v-card-title>
          <v-card-text>
            <v-text-field
              v-model="topicName"
              :label="$t('core.name')"
              box
              required
            />
          </v-card-text>
          <v-card-actions>
            <v-spacer />
            <v-btn
              flat
              color="black"
              @click="newTopicDialog = false"
              v-t="'core.close'"
            />
            <v-btn
              :disabled="topicName == ''"
              flat
              color="primary"
              @click="createTopic"
              v-t="'core.save'"
              type="submit"
            />
          </v-card-actions>
        </v-form>
      </v-card>
    </v-dialog>
  </v-layout>
</template>

<script lang="ts">
import KnowledgeBaseModule from '@/store/modules/knowledgeBase'
import { getModule } from 'vuex-module-decorators'
import { Vue, Component, Prop } from 'vue-property-decorator'
import { Topic } from '../../models/definitions'
import i18n from '@/i18n'

@Component
export default class NewTopicDialog extends Vue {
  private knowledgeBaseModule: KnowledgeBaseModule = getModule(KnowledgeBaseModule, this.$store)

  private form: any = null
  private newTopicDialog: any = null
  private topicName: string = ''

  private async createTopic() {
    const projectId = this.$route.params.projectId

    await this.knowledgeBaseModule.createTopic(new Topic(projectId, this.topicName))

    this.newTopicDialog = false
  }
}
</script>
