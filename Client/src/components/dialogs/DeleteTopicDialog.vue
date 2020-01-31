<template>
  <v-dialog
    v-model="dialog"
    max-width="500px"
  >
    <template v-slot:activator="{ on }">
      <v-btn
        v-on="on"
        text
        color="error"
        v-t="'project.knowledgeBase.deleteTopic'"
        @click="opened"
      />
    </template>
    <v-card>
      <v-form v-model="valid">
        <v-card-title>
          <span
            class="headline"
            v-t="'project.knowledgeBase.deleteTopic'"
          />
        </v-card-title>
        <v-card-text>
          <p
            class="subtitle-1"
            v-t="'project.knowledgeBase.deleteTopicDescription'"
          />
          <p v-t="'project.knowledgeBase.deleteTopicHint'" />
          <v-text-field
            v-model="enteredName"
            :label="$t('core.name')"
            :rules="enteredNameRules"
            filled
            required
          />
        </v-card-text>
        <v-card-actions>
          <v-btn
            text
            v-t="'core.cancel'"
            @click.stop="dialog = false"
          />
          <v-spacer />
          <v-btn
            type="submit"
            text
            color="error"
            v-t="'core.delete'"
            @click.stop="deleteTopic"
            :disabled="!valid"
            :loading="loading"
          />
        </v-card-actions>
      </v-form>
    </v-card>
  </v-dialog>
</template>

<script lang="ts">
import { getModule } from 'vuex-module-decorators'
import { Project, Topic } from '../../models'
import { Component, Watch, Prop, Vue } from 'vue-property-decorator'
import i18n from '../../i18n'
import KnowledgeBaseModule from '../../store/knowledgeBase'

@Component
export default class DeleteTopicDialog extends Vue {
  private knowledgeBaseModule = getModule(KnowledgeBaseModule, this.$store)

  @Prop(Topic)
  private readonly topic?: Topic

  private valid: any = false
  private dialog: any = false
  private loading = false

  private topicName: string = ''
  private enteredName: string = ''
  private get enteredNameRules() {
    return [
      (v: string) => !!v || i18n.t('core.fieldRequired'),
      (v: string) => v === this.topicName || i18n.t('project.knowledgeBase.topicNameMustEqual'),
    ]
  }

  private async opened() {
    if (this.topic) {
      this.topicName = this.topic.name
    }
  }

  private async deleteTopic() {
    this.loading = true

    if (this.topic) {
      await this.knowledgeBaseModule.deleteTopic(this.topic.id)
    }

    this.loading = false
    this.dialog = false
  }

}
</script>
