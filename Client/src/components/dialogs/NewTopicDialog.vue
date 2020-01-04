<template>
  <v-dialog
    v-model="newTopicDialog"
    max-width="600px"
  >
    <template v-slot:activator="{ on }">
      <v-btn
        v-on="on"
        text
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
            filled
            required
          />
        </v-card-text>
        <v-card-actions>
          <v-spacer />
          <v-btn
            text
            @click.stop="newTopicDialog = false"
            v-t="'core.close'"
          />
          <v-btn
            :disabled="topicName == ''"
            text
            color="primary"
            @click.stop="createTopic"
            v-t="'core.save'"
            type="submit"
          />
        </v-card-actions>
      </v-form>
    </v-card>
  </v-dialog>
</template>

<script lang="ts">
import KnowledgeBaseModule from '../../store/knowledgeBase'
import { getModule } from 'vuex-module-decorators'
import { Vue, Component, Prop } from 'vue-property-decorator'
import { Topic } from '../../models/definitions'
import i18n from '../../i18n'

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
