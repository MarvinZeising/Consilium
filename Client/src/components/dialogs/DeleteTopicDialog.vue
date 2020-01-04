<template>
  <v-dialog
    v-model="deleteTopicDialog"
    max-width="600px"
  >
    <template v-slot:activator="{ on }">
      <v-btn
        v-on="on"
        icon
        class="ma-0"
      >
        <v-icon color="grey">delete</v-icon>
      </v-btn>
    </template>
    <v-card>
      <v-form v-model="valid">
        <v-card-title>
          <span
            class="headline"
            v-t="'knowledgeBase.deleteTopic'"
          />
        </v-card-title>
        <v-card-text>
          <p
            class="subtitle-1"
            v-t="'knowledgeBase.deleteTopicDescription'"
          />
          <p
            class="subtitle-1"
            v-t="'knowledgeBase.deleteTopicHint'"
          />
          <v-text-field
            v-model="enteredName"
            :label="$t('core.name')"
            :rules="enteredNameRules"
            filled
            required
          />
        </v-card-text>
        <v-card-actions>
          <v-spacer />
          <v-btn
            text
            @click.stop="deleteTopicDialog = false"
            v-t="'core.cancel'"
          />
          <v-btn
            :disabled="!valid"
            type="submit"
            text
            color="error"
            @click.stop="deleteTopic"
            v-t="'core.delete'"
          />
        </v-card-actions>
      </v-form>
    </v-card>
  </v-dialog>
</template>

<script lang="ts">
import { getModule } from 'vuex-module-decorators'
import { Project, Topic } from '../../models/definitions'
import { Component, Watch, Prop, Vue } from 'vue-property-decorator'
import { VForm } from 'vuetify/lib'
import i18n from '../../i18n'
import KnowledgeBaseModule from '../../store/knowledgeBase'

@Component
export default class DeleteProjectDialog extends Vue {
  private knowledgeBaseModule: KnowledgeBaseModule = getModule(KnowledgeBaseModule, this.$store)

  @Prop(String)
  private readonly topicId: string | undefined

  private valid: any = false
  private deleteTopicDialog: boolean = false
  private topicName: string = ''
  private enteredName: string = ''
  private get enteredNameRules() {
    return [
      (v: string) => !!v || i18n.t('core.fieldRequired'),
      (v: string) => v === this.topicName || i18n.t('knowledgeBase.topicNameMustEqual'),
    ]
  }

  private async created() {
    await this.loadTopic()
  }

  private async loadTopic() {
    const topic = this.knowledgeBaseModule.allTopics.filter((x: Topic) => x.id === this.topicId)[0]
    if (topic) {
      this.topicName = topic.name
    }
  }

  private async deleteTopic() {
    if (this.topicId) {
      await this.knowledgeBaseModule.deleteTopic(this.topicId)
    }

    this.deleteTopicDialog = false
  }

}
</script>
